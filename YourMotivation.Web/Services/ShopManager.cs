using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ORM;
using ORM.Models;
using YourMotivation.Web.Models.CartViewModels;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Models.ShopViewModels;

namespace YourMotivation.Web.Services
{
  public class ShopManager
  {
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<ShopManager> _localizer;

    public ShopManager(
      ApplicationDbContext context,
      IStringLocalizer<ShopManager> localizer)
    {
      _context = context;
      _localizer = localizer;
    }

    public async Task<ShopItemsPageViewModel> GetShopItemPageAsync(int index, int pageSize, string titleFilter)
    {
      var query = _context.Items.AsNoTracking();

      if (!string.IsNullOrWhiteSpace(titleFilter))
      {
        query = query.Where(item => item.Title.Contains(titleFilter));
      }

      var result = new ShopItemsPageViewModel
      {
        CurrentPage = index,
        PageSize = pageSize,
        TitleFilter = titleFilter,
      };

      var totalCount = await query.CountAsync();
      result.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

      query = query
        .Skip((index - 1) * pageSize)
        .Take(pageSize);

      result.Records = await query.Include(i => i.Characteristics).Select(item => ShopItemViewModel.Map(item)).ToListAsync();

      return result;
    }

    public async Task<CartViewModel> FindCartByIdAsync(Guid id)
    {
      var cart = await _context.Carts
        .AsNoTracking()
        .Include(c => c.User)
        .Include(c => c.CartItems)
          .ThenInclude(ci => ci.Item)
            .ThenInclude(i => i.Characteristics)
        .FirstOrDefaultAsync(c => c.Id == id);

      return CartViewModel.Map(cart);
    }

    public async Task<(byte[] Content, string ContentMimeType)> GetItemImageAsync(Guid id)
    {
      var item = await _context.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(i => i.Id == id);

      return (item?.Image, item?.ImageContentType);
    }

    public async Task<(IdentityResult Result, string Title)> AddItemToCartAsync(Guid cartId, Guid itemId)
    {
      var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
      var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

      if (cart == null)
      {
        return
        (Result: IdentityResult.Failed(new IdentityError
        {
          Code = nameof(ShopManager.AddItemToCartAsync),
          Description = _localizer["Error: not found cart by id: '{0}'.", cartId]
        }),
        Title: null);
      }

      if (item == null)
      {
        return 
        (Result: IdentityResult.Failed(new IdentityError
        {
          Code = nameof(ShopManager.AddItemToCartAsync),
          Description = _localizer["Error: not found item by id: '{0}'.", itemId]
        }),
        Title: null);
      }

      try
      {
        var cartItem = 
          await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ItemId == itemId);
        if (cartItem == null)
        {
          await _context.CartItems.AddAsync(new CartItem
          {
            Cart = cart,
            Item = item
          });
        }
        else
        {
          cartItem.Count += 1;
          _context.CartItems.Update(cartItem);
        }

        await _context.SaveChangesAsync();
        return (Result: IdentityResult.Success, Title: item.Title);
      }
      catch(DbUpdateException ex)
      {
        return 
        (Result: IdentityResult.Failed(new IdentityError
        {
          Code = nameof(ShopManager.AddItemToCartAsync),
          Description = ex.InnerException.Message
        }),
        Title: null);
      }
      catch(Exception)
      {
        return (Result: null, Title: null);
      }
    }
  }
}
