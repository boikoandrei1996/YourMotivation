using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;
using YourMotivation.Web.Models.OrderViewModels;
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.Pages;

namespace YourMotivation.Web.Services
{
  public class OrderManager
  {
    private readonly ApplicationDbContext _context;

    public OrderManager(
      ApplicationDbContext context)
    {
      _context = context;
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

    public async Task<bool?> CloseOrderAsync(Guid orderId)
    {
      var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
      if (order == null)
      {
        return null;
      }

      if (order.DateOfClosing.HasValue)
      {
        return true;
      }

      try
      {
        order.DateOfClosing = DateTime.UtcNow;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
