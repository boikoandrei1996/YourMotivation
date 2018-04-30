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
        logger.LogError(ex.InnerException.InnerException, nameof(SeedCartItems.SeedAsync));
      }
    }

    private static async Task<IList<CartItem>> GetCartItemsAsync(ApplicationDbContext context)
    {
      var users = await context.Users.ToArrayAsync();
      var items = await context.Items.ToArrayAsync();

      return new List<CartItem>
      {
        new CartItem
        {
          CartId = users[0].CartId,
          ItemId = items[0].Id
        },
        new CartItem
        {
          CartId = users[0].CartId,
          ItemId = items[3].Id
        },
        new CartItem
        {
          CartId = users[0].CartId,
          ItemId = items[2].Id
        },
        new CartItem
        {
          CartId = users[1].CartId,
          ItemId = items[0].Id
        },
        new CartItem
        {
          CartId = users[1].CartId,
          ItemId = items[1].Id
        },
        new CartItem
        {
          CartId = users[2].CartId,
          ItemId = items[3].Id
        },
        new CartItem
        {
          CartId = users[3].CartId,
          ItemId = items[4].Id
        }
      };
    }
  }
}
