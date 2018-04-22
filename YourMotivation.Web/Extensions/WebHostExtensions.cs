using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ORM;
using ORM.Models;
using YourMotivation.Web.Controllers;

namespace YourMotivation.Web.Extensions
{
  public static class WebHostExtensions
  {
    public static IWebHost SeedData(this IWebHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        var logger = scope.ServiceProvider.GetService<ILogger<Startup>>();
        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        DataSeeder.SeedAsync(userManager, roleManager, logger).Wait();
      }

      return host;
    }
  }
}
