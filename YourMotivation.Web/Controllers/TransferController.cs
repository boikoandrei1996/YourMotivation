using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Models.AccountViewModels;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Services;

namespace YourMotivation.Web.Controllers
{
  [Authorize]
  public class TransferController : Controller
  {
    private readonly TransferManager _transferManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<TransferController> _localizer;

    public TransferController(
      TransferManager transferManager,
      UserManager<ApplicationUser> userManager,
      IStringLocalizer<TransferController> localizer
    )
    {
      _transferManager = transferManager;
      _userManager = userManager;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [TempData]
    public string FormErrorMessage { get; set; }

    // GET: Transfer/All
    public async Task<IActionResult> All(int? pageIndex, NewTransferViewModel model)
    {
      var page =
        await _transferManager.GetTransferPageAsync(pageIndex ?? 1, 4);

      model.FormErrorMessage = this.FormErrorMessage;
      page.NewTransferModel = model;
      page.StatusMessage = this.StatusMessage;

      return View(page);
    }

    // POST: Transfer/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
      int? pageIndex, 
      [Bind(Prefix = "NewTransferModel")] NewTransferViewModel model)
    {
      var routeData = new
      {
        pageIndex,
        model.ReceiverUsername,
        model.Points,
        model.Message
      };

      if (!ModelState.IsValid)
      {
        var pointsErrors = ModelState["NewTransferModel.Points"].Errors;
        if (pointsErrors.Any())
        {
          var messages = string.Join(string.Empty, pointsErrors.Select(err => err.ErrorMessage));
          this.FormErrorMessage = $"{_localizer["Error:"]} {messages}";
        }

        return RedirectToAction(nameof(TransferController.All), routeData);
      }

      var sender = await _userManager.GetUserAsync(User);
      if (sender == null)
      {
        return NotFound();
      }

      var result = await _transferManager.CreateNewTransferAsync(sender.Id, model);
      if (result == null)
      {
        return NotFound();
      }
      else if (result.Succeeded)
      {
        this.FormErrorMessage = _localizer["Success: transfer has been sended."];
        return RedirectToAction(nameof(TransferController.All), new { pageIndex });
      }
      else
      {
        this.FormErrorMessage = result.Errors.Single().Description;
        return RedirectToAction(nameof(TransferController.All), routeData);
      }
    }

    // GET: Transfer/Usernames
    [ActionName("Usernames")]
    public async Task<JsonResult> AutocompleteSearchUsernames(string term)
    {
      var usernames = await _transferManager.GetUsernamesAsync(term, 3, User.Identity.Name);

      var result = usernames.Select(username => new { value = username });

      return Json(result);
    }
  }
}