using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Controllers
{
  [Route("Admin/Users/[action]")]
  // [Authorize(Roles = RoleNames.Admin)]
  public class AdminUsersController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<AccountController> _localizer;

    public AdminUsersController(
      UserManager<ApplicationUser> userManager,
      ILogger<AccountController> logger,
      IStringLocalizer<AccountController> localizer)
    {
      _userManager = userManager;
      _logger = logger;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    // GET: Admin/Users/All
    public async Task<IActionResult> All(
      int? pageIndex, string searchByUsername,
      string sortColumn, bool? orderBy)
    {
      var page = await _userManager.GetUserPageAsync(
        pageIndex ?? 1, 3, searchByUsername, sortColumn, orderBy ?? false);

      ViewBag.SortColumnParam = sortColumn;
      ViewBag.OrderByParm = orderBy ?? false;
      ViewBag.CurrentFilter = searchByUsername;
      page.StatusMessage = this.StatusMessage;

      return View(page);
    }

    // GET: Admin/Users/Details/5
    public async Task<IActionResult> Details(string id)
    {
      var applicationUser = await this.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      return View(applicationUser);
    }

    // GET: Admin/Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
      var applicationUser = await this.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      return View(applicationUser);
    }

    // POST: Admin/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
      var applicationUser = await this.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      IdentityResult result = await _userManager.DeleteUserAsync(applicationUser);
      if (result == null)
      {
        _logger.LogError(nameof(DeleteConfirmed), applicationUser.Email);
        StatusMessage = _localizer["Error: Internal server error."];
      }
      else if (result.Succeeded)
      {
        _logger.LogInformation($"User '{applicationUser.Email}' has been deleted.");
        StatusMessage = _localizer["User '{0}' has been deleted.", applicationUser.Email];
      }
      else
      {
        _logger.LogError($"Can not delete user: '{applicationUser.Email}'.");
        _logger.LogError(nameof(DeleteConfirmed), result.Errors);
        StatusMessage = _localizer["Error: Can not delete user: '{0}'.", applicationUser.Email];

        return RedirectToAction(nameof(Delete), new { id });
      }

      return RedirectToAction(nameof(All));
    }

    //AddRole

    private async Task<ApplicationUser> FindByIdAsync(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        return null;
      }

      return await _userManager.FindByIdAsync(id);
    }
  }
}
