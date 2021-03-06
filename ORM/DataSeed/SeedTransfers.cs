﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedTransfers
  {
    public static async Task SeedAsync(
      ApplicationDbContext context,
      ILogger logger)
    {
      if (await context.Transfers.AnyAsync())
      {
        logger.LogInformation("Transfers already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create transfers in database.");
      }

      await CreateTransfersAsync(context, logger);
    }

    private static async Task CreateTransfersAsync(ApplicationDbContext context, ILogger logger)
    {
      var transfers = await GetTransfersAsync(context);

      try
      {
        await context.Transfers.AddRangeAsync(transfers);
        await context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create transfers.");
        logger.LogError(ex.InnerException, nameof(SeedTransfers.SeedAsync));
      }
    }

    private static async Task<IList<Transfer>> GetTransfersAsync(ApplicationDbContext context)
    {
      var users = await context.Users.ToArrayAsync();

      return new List<Transfer>
      {
        new Transfer
        {
          Points = 1,
          Text = "Thanks 1",
          DateOfCreation = DateTime.UtcNow.AddDays(-2),
          UserSenderId = users[0].Id,
          UserReceiverId = users[1].Id
        },
        new Transfer
        {
          Points = 2,
          Text = "Thanks 2",
          DateOfCreation = DateTime.UtcNow.AddDays(-1),
          UserSenderId = users[2].Id,
          UserReceiverId = users[1].Id
        },
        new Transfer
        {
          Points = 3,
          Text = "Thanks 3",
          DateOfCreation = DateTime.UtcNow.AddDays(-1),
          UserSenderId = users[3].Id,
          UserReceiverId = users[2].Id
        },
        new Transfer
        {
          Points = 1,
          Text = "Thanks 4",
          DateOfCreation = DateTime.UtcNow.AddHours(-10),
          UserSenderId = users[0].Id,
          UserReceiverId = users[3].Id
        },
        new Transfer
        {
          Points = 2,
          Text = "Thanks 5",
          DateOfCreation = DateTime.UtcNow.AddHours(-4),
          UserSenderId = users[4].Id,
          UserReceiverId = users[0].Id
        }
      };
    }
  }
}
