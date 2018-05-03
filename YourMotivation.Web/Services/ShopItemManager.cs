using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Models.ShopViewModels;

namespace YourMotivation.Web.Services
{
  public class ShopItemManager
  {
    private readonly ApplicationDbContext _context;

    public ShopItemManager(ApplicationDbContext context)
    {
      _context = context;
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

    public async Task<ShopItemViewModel> FindByIdAsync(Guid id)
    {
      var item = await _context.Items
        .AsNoTracking()
        .Include(i => i.Characteristics)
        .FirstOrDefaultAsync(i => i.Id == id);

      return ShopItemViewModel.Map(item);
    }

    public async Task<(byte[] Content, string ContentMimeType)> GetItemImageAsync(Guid id)
    {
      var item = await _context.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(i => i.Id == id);

      if (item == null)
      {
        return (null, null);
      }

      return (item.Image, item.ImageContentType);
    }
  }
}
