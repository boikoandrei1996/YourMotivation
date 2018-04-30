using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ORM;
using ORM.Models;

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
        var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        DataSeeder.SeedAsync(userManager, roleManager, context, logger).Wait();
      }

      return host;
    }
  }
}
