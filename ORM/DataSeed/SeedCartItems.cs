using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedCartItems
  {
    public static async Task SeedAsync(
      ApplicationDbContext context,
      ILogger logger)
    {
      if (await context.CartItems.AnyAsync())
      {
        logger.LogInformation("CartItems already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create cart-items in database.");
      }

      await CreateCartItemsAsync(context, logger);
    }

    private static async Task CreateCartItemsAsync(ApplicationDbContext context, ILogger logger)
    {
      var cartItems = await GetCartItemsAsync(context);

      try
      {
        await context.CartItems.AddRangeAsync(cartItems);
        await context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create cart-items.");
        logger.LogError(ex.InnerException, nameof(SeedCartItems.SeedAsync));
      }
    }

    private static async Task<IList<CartItem>> GetCartItemsAsync(ApplicationDbContext context)
    {
      var carts = await context.Carts.ToArrayAsync();
      var items = await context.Items.ToArrayAsync();

      return new List<CartItem>
      {
        new CartItem
        {
          CartId = carts[0].Id,
          ItemId = items[0].Id,
          Count = 1
        },
        new CartItem
        {
          CartId = carts[0].Id,
          ItemId = items[3].Id,
          Count = 2
        },
        new CartItem
        {
          CartId = carts[0].Id,
          ItemId = items[2].Id,
          Count = 1
        },
        new CartItem
        {
          CartId = carts[1].Id,
          ItemId = items[0].Id,
          Count = 2
        },
        new CartItem
        {
          CartId = carts[1].Id,
          ItemId = items[1].Id,
          Count = 2
        },
        new CartItem
        {
          CartId = carts[2].Id,
          ItemId = items[3].Id,
          Count = 1
        },
        new CartItem
        {
          CartId = carts[3].Id,
          ItemId = items[4].Id,
          Count = 1
        }
      };
    }
  }
}
