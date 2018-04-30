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
      ILogger logger)
    {
      await SeedRoles.SeedAsync(roleManager, logger);
      await SeedUsers.SeedAsync(userManager, logger);
    }
  }
}
