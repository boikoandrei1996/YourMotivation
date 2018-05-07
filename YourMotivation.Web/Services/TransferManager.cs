using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ORM;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Models.TransferViewModels;

namespace YourMotivation.Web.Services
{
  public class TransferManager
  {
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<TransferManager> _localizer;

    public TransferManager(
      ApplicationDbContext context,
      IStringLocalizer<TransferManager> localizer
    )
    {
      _context = context;
      _localizer = localizer;
    }


    public async Task<TransfersPageViewModel> GetTransferPageAsync(int index, int pageSize)
    {
      var query = _context.Transfers
        .AsNoTracking();

      var result = new TransfersPageViewModel
      {
        CurrentPage = index,
        PageSize = pageSize
      };

      var totalCount = await query.CountAsync();
      result.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

      query = query
        .OrderByDescending(t => t.DateOfCreation)
        .Skip((index - 1) * pageSize)
        .Take(pageSize);

      result.Records = await query
        .Include(t => t.UserSender)
        .Include(t => t.UserReceiver)
        .Select(transfer => ShowTransferViewModel.Map(transfer))
        .ToListAsync();

      return result;
    }
  }
}
