using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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

    // GET: Transfer/All
    public async Task<IActionResult> All(int? pageIndex)
    {
      var page =
        await _transferManager.GetTransferPageAsync(pageIndex ?? 1, 4);

      page.StatusMessage = this.StatusMessage;

      return View(page);
    }
  }
}