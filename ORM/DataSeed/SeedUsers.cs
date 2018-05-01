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
      ApplicationDbContext context,
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

      foreach (var model in await GetUserModelsAsync(context))
      {
        await CreateUserAsync(model.User, model.Password, userManager, logger);
        await AddToRoleAsync(model.User, model.Role, userManager, logger);
      }
    }

    private static async Task CreateUserAsync(
      ApplicationUser user, 
      string password,
      UserManager<ApplicationUser> userManager, 
      ILogger logger)
    {
      IdentityResult result = await userManager.CreateAsync(user, password);
      if (!result.Succeeded)
      {
        logger.LogError($"Can not create user: '{user.Email}'.");
        logger.LogError(nameof(IdentityResult.Errors), result.Errors);
      }
    }

    private static async Task AddToRoleAsync(
      ApplicationUser user, 
      string role,
      UserManager<ApplicationUser> userManager, 
      ILogger logger)
    {
      IdentityResult result = await userManager.AddToRoleAsync(user, role);
      if (!result.Succeeded)
      {
        logger.LogError($"Can not add role '{role}' to user: '{user.Email}'.");
        logger.LogError(nameof(IdentityResult.Errors), result.Errors);
      }
    }

    private static async Task<IList<(ApplicationUser User, string Password, string Role)>> GetUserModelsAsync(ApplicationDbContext context)
    {
      var userPassword = "User123!";
      var adminPassword = "Admin123!";

      var userModels = new List<(ApplicationUser User, string Password, string Role)>
      {
        ValueTuple.Create(new ApplicationUser
        {
          Email = "admin@mail.ru",
          PhoneNumber = "375291234567",
          CreatedDate = DateTime.UtcNow.AddDays(-3)
        },
        adminPassword,
        ApplicationRole.Admin),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "moderator@mail.ru",
          PhoneNumber = "375291234567",
          CreatedDate = DateTime.UtcNow.AddDays(-3)
        },
        userPassword,
        ApplicationRole.Moderator),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user1@mail.ru",
          PhoneNumber = "375291234567",
          CreatedDate = DateTime.UtcNow.AddHours(-5)
        },
        userPassword,
        ApplicationRole.User),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user2@mail.ru",
          CreatedDate = DateTime.UtcNow.AddDays(-1)
        },
        userPassword,
        ApplicationRole.User),

        ValueTuple.Create(new ApplicationUser
        {
          Email = "user3@mail.ru",
          CreatedDate = DateTime.UtcNow.AddDays(-2)
        },
        userPassword,
        ApplicationRole.User)
      };

      foreach (var model in userModels)
      {
        model.User.UserName = model.User.Email.Split('@')[0];
        model.User.EmailConfirmed = true;
        model.User.TotalPoints = 20;
      }

      return userModels;
    }
  }
}
