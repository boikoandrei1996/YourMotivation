using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ORM;

namespace YourMotivation.Web.Data
{
  public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var connectionString = configuration.GetConnectionString("DefaultConnection");
      var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

      builder.UseSqlServer(connectionString);

      return new ApplicationDbContext(builder.Options);
    }
  }
}
