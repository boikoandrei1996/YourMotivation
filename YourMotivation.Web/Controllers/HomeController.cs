using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ORM;
using YourMotivation.Web.Models;

namespace YourMotivation.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IStringLocalizer<HomeController> _localizer;
    private readonly ApplicationDbContext _context;

    public HomeController(
      IStringLocalizer<HomeController> localizer,
      ApplicationDbContext context)
    {
      _localizer = localizer;
      _context = context;
    }

    public IActionResult Index()
    {
      var users = _context.Users.AsNoTracking().ToList();
      var users2 = _context.Users.Include(e => e.Cart).AsNoTracking().ToList();
      var users3 = _context.Users.Include(e => e.Orders).AsNoTracking().ToList();
      var users4 = _context.Users
        .Include(e => e.TransferAsSender)
        .Include(e => e.TransfersAsReceiver)
        .AsNoTracking().ToList();
      var roles = _context.Roles.AsNoTracking().ToList();
      var carts = _context.Carts.AsNoTracking().ToList();
      var items = _context.Items.AsNoTracking().ToList();
      var orders = _context.Orders.AsNoTracking().ToList();
      var orders2 = _context.Orders.Include(e => e.User).AsNoTracking().ToList();
      var orders3 = _context.Orders.Include(e => e.Cart).AsNoTracking().ToList();
      var transfers = _context.Transfers.AsNoTracking().ToList();

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
