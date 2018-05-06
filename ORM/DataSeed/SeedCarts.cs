using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedCarts
  {
    public static async Task SeedAsync(
      ApplicationDbContext context,
      ILogger logger)
    {
      if (await context.Orders.AnyAsync())
      {
        logger.LogInformation("Carts already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create carts in database.");
      }

      await CreateCartsAsync(context, logger);
    }

    private static async Task CreateCartsAsync(ApplicationDbContext context, ILogger logger)
    {
      var carts = GetCarts(context);

      try
      {
        await context.Carts.AddRangeAsync(carts);
        await context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create carts.");
        logger.LogError(ex.InnerException, nameof(SeedCarts.SeedAsync));
      }
    }

    private static IList<Cart> GetCarts(ApplicationDbContext context)
    {
      var carts = new List<Cart>();

      for (int i = 0; i < 5; i++)
      {
        carts.Add(new Cart
        {
          UserId = null
        });
      }

      return carts;
    }
  }
}
