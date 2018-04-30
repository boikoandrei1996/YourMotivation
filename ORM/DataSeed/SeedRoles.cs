using System;
using System.Collections.Generic;
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

      foreach (var role in GetRoles())
      {
        await CreateRoleAsync(role, roleManager, logger);
      }
    }

    private static async Task CreateRoleAsync(
      ApplicationRole role,
      RoleManager<ApplicationRole> roleManager,
      ILogger logger)
    {
      try
      {
        IdentityResult result = await roleManager.CreateAsync(role);
        if (result.Succeeded == false)
        {
          logger.LogError($"Can not create role: '{role.Name}'.");
          logger.LogError(nameof(IdentityResult.Errors), result.Errors);
        }
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create role: '{role.Name}'.");
        logger.LogError(ex.InnerException.InnerException, nameof(SeedUsers.SeedAsync));
      }
    }

    private static IList<ApplicationRole> GetRoles()
    {
      return new List<ApplicationRole>
      {
        new ApplicationRole { Name = ApplicationRole.Admin },
        new ApplicationRole { Name = ApplicationRole.Moderator },
        new ApplicationRole { Name = ApplicationRole.User }
      };
    }
  }
}
