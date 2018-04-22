using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedUsers
  {
    public static async Task SeedAsync(
      UserManager<ApplicationUser> userManager,
      ILogger logger)
    {
      if (await userManager.Users.AnyAsync())
      {
        logger.LogInformation("Users already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create users in database.");
      }

      foreach (var model in GetUserModels())
      {
        await CreateUserAsync(model.User, model.Password, userManager, logger);
        await AddToRoleAsync(model.User, model.Role, userManager, logger);
      }
    }

    private static async Task CreateUserAsync(
      ApplicationUser user, string password, 
      UserManager<ApplicationUser> userManager, ILogger logger)
    {
      string errorMessage = $"Can not create user: '{user.Email}'.";
      try
      {
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (result.Succeeded == false)
        {
          logger.LogError(errorMessage);
          logger.LogError(nameof(IdentityResult.Errors), result.Errors);
        }
      }
      catch (DbUpdateException ex)
      {
        logger.LogError(errorMessage);
        logger.LogError(ex.InnerException.InnerException, nameof(SeedUsers.SeedAsync));
      }
    }

    private static async Task AddToRoleAsync(
      ApplicationUser user, string role,
      UserManager<ApplicationUser> userManager, ILogger logger
    )
    {
      string errorMessage = $"Can not add role '{role}' to user: '{user.Email}'.";
      try
      {
        IdentityResult result = await userManager.AddToRoleAsync(user, role);
        if (result.Succeeded == false)
        {
          logger.LogError(errorMessage);
          logger.LogError(nameof(IdentityResult.Errors), result.Errors);
        }
      }
      catch (DbUpdateException ex)
      {
        logger.LogError(errorMessage);
        logger.LogError(ex.InnerException.InnerException, nameof(SeedUsers.SeedAsync));
      }
    }

    private static IList<(ApplicationUser User, string Password, string Role)> GetUserModels()
    {
      var userPassword = "User123!";
      var adminPassword = "Admin123!";

      var userModels = new List<(ApplicationUser User, string Password, string Role)>
      {
        ValueTuple.Create(new ApplicationUser
        {
          Email = "admin@mail.ru",
          PhoneNumber = "375291234567"
        },
        adminPassword,
        RoleNames.Admin),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "moderator@mail.ru",
          PhoneNumber = "375291234567"
        },
        userPassword,
        RoleNames.Moderator),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user1@mail.ru",
          PhoneNumber = "375291234567"
        },
        userPassword,
        RoleNames.User),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user2@mail.ru",
        },
        userPassword,
        RoleNames.User),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user3@mail.ru",
        },
        userPassword,
        RoleNames.User)
      };

      foreach (var model in userModels)
      {
        model.User.UserName = model.User.Email;
        model.User.EmailConfirmed = true;
      }

      return userModels;
    }
  }
}
