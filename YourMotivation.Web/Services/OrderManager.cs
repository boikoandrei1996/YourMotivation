using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;
using YourMotivation.Web.Models.OrderViewModels;
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.Pages;
using ORM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace YourMotivation.Web.Services
{
  public class OrderManager
  {
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<OrderManager> _localizer;

    public OrderManager(
      ApplicationDbContext context,
      IStringLocalizer<OrderManager> localizer)
    {
      _context = context;
      _localizer = localizer;
    }

    public async Task<OrdersPageViewModel> GetOrderPageAsync(
      int index, int pageSize, SortState sortState, Guid? ownerUserId)
    {
      var query = _context.Orders.AsNoTracking();

      if (ownerUserId.HasValue)
      {
        query = query.Where(o => o.UserId == ownerUserId.Value);
      }

      switch (sortState)
      {
        case SortState.DateOfClosingAsc:
          query = query.OrderBy(user => user.DateOfClosing);
          break;
        case SortState.DateOfClosingDesc:
          query = query.OrderByDescending(user => user.DateOfClosing);
          break;
        case SortState.CreatedDateAsc:
          query = query.OrderBy(user => user.DateOfCreation);
          break;
        case SortState.CreatedDateDesc:
          query = query.OrderByDescending(user => user.DateOfCreation);
          break;
      }

      var result = new OrdersPageViewModel
      {
        CurrentPage = index,
        PageSize = pageSize,
        SortViewModel = new SortOrderViewModel(sortState)
      };

      var totalCount = await query.CountAsync();
      result.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

      query = query
        .Skip((index - 1) * pageSize)
        .Take(pageSize);

      result.Records = await query
        .Include(o => o.User)
        .Select(order => ShowOrderViewModel.Map(order))
        .ToListAsync();

      return result;
    }

    public async Task<IList<OrderItemViewModel>> GetOrderItemsAsync(Guid cartId)
    {
      var cart = await _context.Carts
        .AsNoTracking()
        .Include(c => c.CartItems)
          .ThenInclude(ci => ci.Item)
        .FirstOrDefaultAsync(c => c.Id == cartId);

      var items = new List<OrderItemViewModel>();
      foreach (var cartItem in cart.CartItems)
      {
        items.Add(OrderItemViewModel.Map(cartItem.Item, cartItem.Count));
      }

      return items;
    }

    public async Task<IdentityResult> CloseOrderAsync(Guid orderId)
    {
      var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
      if (order == null)
      {
        return null;
      }

      if (order.DateOfClosing.HasValue)
      {
        return IdentityResult.Success;
      }

      try
      {
        order.DateOfClosing = DateTime.UtcNow;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CloseOrderAsync),
          Description = ex.InnerException.Message
        });
      }
      catch (Exception)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CloseOrderAsync),
          Description = _localizer["Error: something has gone wrong while closing order."]
        });
      }
    }

    public async Task<IdentityResult> RemoveOrderAsync(Guid orderId)
    {
      var order = await _context.Orders.Include(o => o.Cart).FirstOrDefaultAsync(o => o.Id == orderId);
      if (order == null)
      {
        return null;
      }

      if (order.DateOfClosing.HasValue)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.RemoveOrderAsync),
          Description = _localizer["Error: You can not remove closed order."]
        });
      }

      try
      {
        _context.Carts.Remove(order.Cart);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.RemoveOrderAsync),
          Description = ex.InnerException.Message
        });
      }
      catch (Exception)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.RemoveOrderAsync),
          Description = _localizer["Error: something has gone wrong while removing order."]
        });
      }
    }

    public async Task<IdentityResult> CreateNewOrderAsync(Guid userId)
    {
      var user = await _context.Users
        .Include(u => u.Cart)
          .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Item)
        .FirstOrDefaultAsync(u => u.Id == userId);

      if (user == null)
      {
        return null;
      }

      var cartPoints = user.Cart.CartItems.Sum(ci => ci.Item.Price * ci.Count);

      var checkResult = this.CheckOrderIsPossible(user, cartPoints);
      if (!checkResult.Succeeded)
      {
        return checkResult;
      }

      user.TotalPoints -= cartPoints;
      foreach (var cartItem in user.Cart.CartItems)
      {
        cartItem.Item.CountsInStock -= cartItem.Count;
      }

      var oldCart = user.Cart;
      oldCart.UserId = null;
      user.Cart = new Cart();

      var order = new Order
      {
        CartId = oldCart.Id,
        UserId = user.Id,
        DateOfCreation = DateTime.UtcNow
      };

      try
      {
        _context.Carts.Update(oldCart);
        _context.Users.Update(user);
        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync();

        return IdentityResult.Success;
      }
      catch(DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CreateNewOrderAsync),
          Description = ex.InnerException.Message
        });
      }
      catch(Exception)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CreateNewOrderAsync),
          Description = _localizer["Error: something has gone wrong while creating order."]
        });
      }
    }

    private IdentityResult CheckOrderIsPossible(ApplicationUser user, int cartPoints)
    {
      var cartItem = user.Cart.CartItems.FirstOrDefault(ci => ci.Count > ci.Item.CountsInStock);
      if (cartItem != null)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CreateNewOrderAsync),
          Description = _localizer["Error: there are not enough items '{0}' in stock.", cartItem.Item.Title]
        });
      }

      if (cartPoints > user.TotalPoints)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(OrderManager.CreateNewOrderAsync),
          Description = _localizer["Error: you have not enough points."]
        });
      }

      return IdentityResult.Success;
    }
  }
}
