using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using YourMotivation.Web.Models.AccountViewModels;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Services;

namespace YourMotivation.Web.Controllers
{
  [Authorize]
  public class TransferController : Controller
  {
    private readonly TransferManager _transferManager;
    private readonly IStringLocalizer<TransferController> _localizer;

    public TransferController(
      TransferManager transferManager,
      IStringLocalizer<TransferController> localizer
    )
    {
      _transferManager = transferManager;
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
      if (!ModelState.IsValid)
      {
        var routeData = new
        {
          pageIndex,
          model.ReceiverUsername,
          model.Points,
          model.Message
        };

        var pointsErrors = ModelState["NewTransferModel.Points"].Errors;
        if (pointsErrors.Any())
        {
          var messages = string.Join(string.Empty, pointsErrors.Select(err => err.ErrorMessage));
          this.FormErrorMessage = $"{_localizer["Error:"]} {messages}";
        }

        return RedirectToAction("All", routeData);
      }

      return RedirectToAction(nameof(TransferController.All), new { pageIndex });
    }
  }
}