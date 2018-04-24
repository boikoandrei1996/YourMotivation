using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Models.ManageViewModels;

namespace YourMotivation.Web.Controllers
{
  [Authorize]
  [Route("[controller]/[action]")]
  public class ManageController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<ManageController> _localizer;

    public ManageController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      ILogger<ManageController> logger,
      IStringLocalizer<ManageController> localizer)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      var user = await _userManager.GetUserAsync(User);
      this.CheckUserIfNull(user);

      return View(IndexViewModel.Map(user, this.StatusMessage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IndexViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.GetUserAsync(User);
      this.CheckUserIfNull(user);

      if (model.PhoneNumber != user.PhoneNumber)
      {
        var result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
        if (!result.Succeeded)
        {
          this.AddErrors(result);
          return View(model);
        }
      }

      this.StatusMessage = _localizer["Your profile has been updated."];

      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
      var user = await _userManager.GetUserAsync(User);
      this.CheckUserIfNull(user);

      var model = new ChangePasswordViewModel { StatusMessage = this.StatusMessage };

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.GetUserAsync(User);
      this.CheckUserIfNull(user);

      var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
      if (!result.Succeeded)
      {
        this.AddErrors(result);
        return View(model);
      }

      await _signInManager.SignInAsync(user, isPersistent: false);

      this.StatusMessage = _localizer["Your password has been changed."];

      return RedirectToAction(nameof(ChangePassword));
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private void CheckUserIfNull(ApplicationUser user)
    {
      if (user == null)
      {
        throw new ApplicationException(
          _localizer["Unable to load user with ID '{0}'.", _userManager.GetUserId(User)]);
      }
    }
  }
}
