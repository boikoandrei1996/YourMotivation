using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Extensions;
using YourMotivation.Web.Models.AdminViewModels;
using YourMotivation.Web.Models.Pagination;

namespace YourMotivation.Web.Controllers
{
  [Route("[controller]/[action]")]
  // [Authorize(Roles = RoleNames.Admin)]
  public class AdminController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<AdminController> _localizer;

    public AdminController(
      UserManager<ApplicationUser> userManager,
      ILogger<AdminController> logger,
      IStringLocalizer<AdminController> localizer)
    {
      _userManager = userManager;
      _logger = logger;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> Users(int? pageIndex, string usernameFilter, SortState? sortOrder)
    {
      var page = 
        await _userManager.GetUserPageAsync(pageIndex ?? 1, 2, usernameFilter, sortOrder ?? SortState.CreatedDateDesc);

      page.StatusMessage = this.StatusMessage;
      
      return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string id)
    {
      var applicationUser = await this.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      var role = await _userManager.GetUserRoleAsync(applicationUser);
      var model = AdminUserViewModel.Map(applicationUser, role);
      model.StatusMessage = this.StatusMessage;

      return View(model);
    }
    
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

      var result = await _userManager.DeleteUserAsync(applicationUser);
      if (result == null)
      {
        _logger.LogError(nameof(DeleteConfirmed), applicationUser.Email);
        this.StatusMessage = _localizer["Error: Internal server error."];
      }
      else if (result.Succeeded)
      {
        _logger.LogInformation($"User '{applicationUser.Email}' has been deleted.");
        this.StatusMessage = _localizer["Success: User '{0}' has been deleted.", applicationUser.Email];
      }
      else
      {
        _logger.LogError($"Can not delete user: '{applicationUser.Email}'.");
        _logger.LogError(nameof(DeleteConfirmed), result.Errors);
        this.StatusMessage = _localizer["Error: Can not delete user: '{0}'.", applicationUser.Email];
      }

      return RedirectToAction(nameof(Users));
    }
    
    [HttpPost]
    [ActionName("SetRole")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetRole(string id, string newRole)
    {
      var applicationUser = await this.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      if (!RoleNames.GetAllRoles().Contains(newRole))
      {
        return RedirectToAction(nameof(Manage), new { id });
      }

      var oldRole = await _userManager.GetUserRoleAsync(applicationUser);
      if (string.Equals(oldRole, newRole, System.StringComparison.OrdinalIgnoreCase))
      {
        this.StatusMessage = 
          _localizer["Warning: User '{0}' has already role '{1}'.", applicationUser.UserName, newRole];
        return RedirectToAction(nameof(Manage), new { id });
      }

      var result = await _userManager.AddToRoleAsync(applicationUser, newRole);
      if (result.Succeeded)
      {
        result = await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
        if (result.Succeeded)
        {
          this.StatusMessage = _localizer["Success: Role has been updated."];
          return RedirectToAction(nameof(Manage), new { id });
        }
        else
        {
          _logger.LogError($"Can not remove role '{oldRole}' for user '{applicationUser.UserName}'.", result.Errors);
        }
      }
      else
      {
        _logger.LogError($"Can not add role '{newRole}' for user '{applicationUser.UserName}'.", result.Errors);
      }

      this.StatusMessage = 
        _localizer["Error: Can not add role '{0}' for user '{1}'.", newRole, applicationUser.UserName];

      return RedirectToAction(nameof(Manage), new { id });
    }

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
