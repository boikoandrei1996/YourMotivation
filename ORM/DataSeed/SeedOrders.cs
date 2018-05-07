using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedOrders
  {
    public static async Task SeedAsync(
      ApplicationDbContext context,
      ILogger logger)
    {
      if (await context.Orders.AnyAsync())
      {
        logger.LogInformation("Orders already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create orders in database.");
      }

      await CreateOrdersAsync(context, logger);
    }

    private static async Task CreateOrdersAsync(ApplicationDbContext context, ILogger logger)
    {
      var orders = await GetOrdersAsync(context);

      try
      {
        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create orders.");
        logger.LogError(ex.InnerException, nameof(SeedOrders.SeedAsync));
      }
    }

    private static async Task<IList<Order>> GetOrdersAsync(ApplicationDbContext context)
    {
      var users = await context.Users.ToArrayAsync();
      var carts = await context.Carts.Take(5).ToArrayAsync();

      return new List<Order>
      {
        new Order
        {
          DateOfCreation = DateTime.UtcNow.AddDays(-3),
          DateOfClosing = DateTime.UtcNow.AddDays(-1),
          UserId = users[0].Id,
          CartId = carts[0].Id
        },
        new Order
        {
          DateOfCreation = DateTime.UtcNow.AddDays(-2),
          DateOfClosing = DateTime.UtcNow.AddDays(-1),
          UserId = users[1].Id,
          CartId = carts[1].Id
        },
        new Order
        {
          DateOfCreation = DateTime.UtcNow.AddDays(-3),
          UserId = users[2].Id,
          CartId = carts[2].Id
        },
        new Order
        {
          DateOfCreation = DateTime.UtcNow.AddDays(-1),
          UserId = users[3].Id,
          CartId = carts[3].Id
        },
        new Order
        {
          DateOfCreation = DateTime.UtcNow.AddDays(-1),
          DateOfClosing = DateTime.UtcNow,
          UserId = users[4].Id,
          CartId = carts[4].Id
        }
      };
    }
  }
}
