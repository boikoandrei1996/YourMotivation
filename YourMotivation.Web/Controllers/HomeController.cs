using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YourMotivation.Web.Models;

namespace YourMotivation.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IStringLocalizer<HomeController> _localizer;

    public HomeController(IStringLocalizer<HomeController> localizer)
    {
      _localizer = localizer;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult About()
    {
      ViewBag.Message = _localizer.GetString("Your application description page.");

      return View();
    }

    public IActionResult Contact()
    {
      ViewBag.Message = _localizer.GetString("Your contact page.");

      return View();
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
      Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
      );

      return LocalRedirect(returnUrl);
    }
  }
}
