using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ORM.DataSeed;
using ORM.Models;

namespace ORM
{
  public static class DataSeeder
  {
    public static async Task SeedAsync(
      UserManager<ApplicationUser> userManager, 
      RoleManager<ApplicationRole> roleManager,
      ApplicationDbContext context,
      ILogger logger)
    {
      await SeedRoles.SeedAsync(roleManager, logger);
      await SeedCarts.SeedAsync(context, logger);
      await SeedUsers.SeedAsync(userManager, logger);
      await SeedItems.SeedAsync(context, logger);
      await SeedCartItems.SeedAsync(context, logger);
      await SeedOrders.SeedAsync(context, logger);
      await SeedTransfers.SeedAsync(context, logger);
    }
  }
}
