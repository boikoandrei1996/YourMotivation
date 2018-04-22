using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args)
        .SeedData()
        .Run();
    }

    public static IWebHost BuildWebHost(string[] args) => 
      WebHost
        .CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
  }
}
