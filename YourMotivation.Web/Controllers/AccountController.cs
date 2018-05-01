using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Extensions;
using YourMotivation.Web.Models.AccountViewModels;
using YourMotivation.Web.Services;

namespace YourMotivation.Web.Controllers
{
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<AccountController> _localizer;
    private readonly IHtmlLocalizer<EmailSender> _emailLocalizer;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        IStringLocalizer<AccountController> localizer,
        IHtmlLocalizer<EmailSender> emailLocalizer)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
      _localizer = localizer;
      _emailLocalizer = emailLocalizer;
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();

      _logger.LogInformation($"User '{User.Identity.Name}' logged out.");

      return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
      ViewBag.ReturnUrl = returnUrl;

      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
      ViewBag.ReturnUrl = returnUrl;

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await _signInManager.PasswordSignInAsync(
        model.Email.GetUsernameFromEmail(), model.Password, model.RememberMe, false);
      if (!result.Succeeded)
      {
        ModelState.AddModelError(string.Empty, _localizer.GetString("Invalid login attempt."));
        return View(model);
      }

      _logger.LogInformation($"User '{model.Email}' logged in.");

      return RedirectToLocal(returnUrl);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl = null)
    {
      ViewBag.ReturnUrl = returnUrl;

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
      ViewBag.ReturnUrl = returnUrl;

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = RegisterViewModel.Map(model);

      var result = await _userManager.CreateUserWithRoleAsync(user, model.Password);
      if (result.Succeeded)
      {
        _logger.LogInformation($"User '{user.UserName}' created a new account with password.");

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
        var isSuccess = await _emailSender.SendEmailConfirmationAsync(_emailLocalizer, model.Email, callbackUrl);

        return RedirectToLocal(returnUrl, Url.Action(nameof(AccountController.Login), "Account"));
      }
      else
      {
        this.AddErrors(result);
        return View(model);
      }
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
      if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        // return NotFound();
        throw new ApplicationException(
          _localizer["Unable to load user with ID '{0}'.", userId]);
      }

      if (user.EmailConfirmed)
      {
        return RedirectToAction(nameof(AccountController.Login), "Account");
      }

      var result = await _userManager.ConfirmEmailAsync(user, code);

      return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null || !user.EmailConfirmed)
      {
        return RedirectToAction(nameof(ForgotPassword));
      }

      var code = await _userManager.GeneratePasswordResetTokenAsync(user);
      var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
      var isSuccess = await _emailSender.SendEmailResetPasswordAsync(_emailLocalizer, model.Email, callbackUrl);

      return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string code = null)
    {
      if (string.IsNullOrEmpty(code))
      {
        throw new ApplicationException(
          _localizer.GetString("A code must be supplied for password reset."));
      }

      var model = new ResetPasswordViewModel { Code = code };

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
      {
        return RedirectToAction(nameof(ResetPassword));
      }

      var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
      if (!result.Succeeded)
      {
        this.AddErrors(result);
        return View();
      }

      return RedirectToAction(nameof(ResetPasswordConfirmation));
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
      return View();
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private IActionResult RedirectToLocal(string returnUrl, string defaultUrl = null)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else if (defaultUrl != null && Url.IsLocalUrl(defaultUrl))
      {
        return Redirect(defaultUrl);
      }
      else
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }
    }
  }
}
