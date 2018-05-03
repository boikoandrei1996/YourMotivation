using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ORM;
using ORM.Models;
using YourMotivation.Web.Models;
using YourMotivation.Web.Services;

namespace YourMotivation.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      services
        .AddIdentity<ApplicationUser, ApplicationRole>(config =>
        {
          config.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      // Add the localization services to the services container.
      services.AddLocalization(options => options.ResourcesPath = "Resources");

      services
        .AddMvc()
        .AddViewLocalization()
        .AddDataAnnotationsLocalization(options =>
          options.DataAnnotationLocalizerProvider = (type, factory) => 
          {
            return factory.Create(typeof(ValidationMessages));
          }
        );

      // Add application services.
      services.AddTransient<IEmailSender, EmailSender>();
      services.AddTransient<ShopItemManager>();

      // Configure supported cultures and localization options
      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new[]
        {
          new CultureInfo("en"),
          new CultureInfo("ru")
        };

        options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      });

      services.Configure<AuthMessageSenderOptions>(Configuration);

      /*services.AddElm(options =>
      {
        options.Path = new PathString("/elmah");
        options.Filter = (name, level) => level >= LogLevel.Error;
      });*/
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app, 
      IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseRequestLocalization(
        app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

      app.UseStaticFiles();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

      // app.UseElmPage(); // Shows the logs at the specified path
      // app.UseElmCapture(); // Adds the ElmLoggerProvider
    }
  }
}
