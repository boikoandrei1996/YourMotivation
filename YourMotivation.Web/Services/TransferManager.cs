using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ORM;
using ORM.Models;
using YourMotivation.Web.Models.AccountViewModels;
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

    public Task<List<string>> GetUsernamesAsync(string filter, int count, string currentUser)
    {
      return _context.Users
        .AsNoTracking()
        .Where(u =>
          u.UserName.StartsWith(filter, StringComparison.OrdinalIgnoreCase) &&
          !u.UserName.Equals(currentUser, StringComparison.OrdinalIgnoreCase))
        .Take(count)
        .Select(u => u.UserName)
        .ToListAsync();
    }

    public Task<List<TopUser>> GetTopReceivers(FilterForTopUsers filter, int count)
    {
      var query = _context.Transfers.AsNoTracking();

      switch (filter)
      {
        case (FilterForTopUsers.AllTime):
          break;
        case (FilterForTopUsers.ByYear):
          query = query.Where(t => t.DateOfCreation >= DateTime.UtcNow.AddYears(-1));
          break;
        case (FilterForTopUsers.ByMonth):
          query = query.Where(t => t.DateOfCreation >= DateTime.UtcNow.AddMonths(-1));
          break;
        case (FilterForTopUsers.ByDay):
          query = query.Where(t => t.DateOfCreation >= DateTime.UtcNow.AddDays(-1));
          break;
      }

      var resultQuery = query
        .GroupBy(
          t => t.UserReceiverId,
          (userId, transfers) => new { id = userId, count = transfers.Count() }
        )
        .OrderByDescending(x => x.count)
        .Take(count)
        .Join(
          _context.Users.AsNoTracking(),
          u1 => u1.id, u2 => u2.Id,
          (u1, u2) => new TopUser { Id = u1.id, Count = u1.count, Username = u2.UserName }
        );

      return resultQuery.ToListAsync();
    }

    public async Task<IdentityResult> CreateNewTransferAsync(Guid senderId, NewTransferViewModel model)
    {
      var sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderId);
      if (sender == null)
      {
        return null;
      }

      var receiver = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.ReceiverUsername);

      var checkResult = this.CheckTransferIsPossible(sender, receiver, model);
      if (!checkResult.Succeeded)
      {
        return checkResult;
      }

      var newTransfer = new Transfer
      {
        Text = model.Message,
        Points = model.Points,
        DateOfCreation = DateTime.UtcNow,
        UserSenderId = sender.Id,
        UserReceiverId = receiver.Id
      };

      try
      {
        sender.PointsPerMonth -= model.Points;
        receiver.TotalPoints += model.Points;

        await _context.Transfers.AddAsync(newTransfer);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(TransferManager.CreateNewTransferAsync),
          Description = ex.InnerException.Message
        });
      }
      catch (Exception)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(TransferManager.CreateNewTransferAsync),
          Description = _localizer["Error: something has gone wrong while creating transfer."]
        });
      }
    }

    private IdentityResult CheckTransferIsPossible(
      ApplicationUser sender,
      ApplicationUser receiver,
      NewTransferViewModel model)
    {
      if (receiver == null)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(TransferManager.CreateNewTransferAsync),
          Description = _localizer["Error: not found user '{0}'.", model.ReceiverUsername]
        });
      }

      if (sender.PointsPerMonth < model.Points)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(TransferManager.CreateNewTransferAsync),
          Description = _localizer["Error: you have not enough points."]
        });
      }

      return IdentityResult.Success;
    }
  }
}
