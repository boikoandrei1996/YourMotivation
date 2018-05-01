using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedRoles
  {
    public static async Task SeedAsync(
      RoleManager<ApplicationRole> roleManager,
      ILogger logger)
    {
      if (await roleManager.Roles.AnyAsync())
      {
        logger.LogInformation("Roles already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create roles in database.");
      }

      var roles = ApplicationRole.GetAllRoles().Select(roleName => new ApplicationRole { Name = roleName });
      foreach (var role in roles)
      {
        await CreateRoleAsync(role, roleManager, logger);
      }
    }

    private static async Task CreateRoleAsync(
      ApplicationRole role,
      RoleManager<ApplicationRole> roleManager,
      ILogger logger)
    {
      IdentityResult result = await roleManager.CreateAsync(role);
      if (!result.Succeeded)
      {
        logger.LogError($"Can not create role: '{role.Name}'.");
        logger.LogError(nameof(IdentityResult.Errors), result.Errors);
      }
    }
  }
}
