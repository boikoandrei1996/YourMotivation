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
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.Pages;
using YourMotivation.Web.Services;

namespace YourMotivation.Web.Controllers
{
  [Authorize]
  public class OrderController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly OrderManager _orderManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<OrderController> _localizer;

    public OrderController(
      UserManager<ApplicationUser> userManager,
      OrderManager orderManager,
      ILogger<ShopController> logger,
      IStringLocalizer<OrderController> localizer)
    {
      _userManager = userManager;
      _orderManager = orderManager;
      _logger = logger;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    // GET Order/All
    public async Task<IActionResult> All(int? pageIndex, SortState? sortOrder)
    {
      var user = await _userManager.FindByNameAsync(User.Identity.Name);
      if (user == null)
      {
        throw new ApplicationException(nameof(OrderController.All));
      }

      Guid? userId = user.Id;
      if (User.IsInRole(ApplicationRole.Moderator))
      {
        userId = null;
        ViewBag.OwnerUsername = string.Empty;
      }
      else
      {
        ViewBag.OwnerUsername = $" {_localizer["by"].Value} {user.UserName}";
      }

      OrdersPageViewModel page = 
        await _orderManager.GetOrderPageAsync(pageIndex ?? 1, 4, sortOrder ?? SortState.CreatedDateDesc, userId);

      page.StatusMessage = this.StatusMessage;

      return View(page);
    }

    // POST Order/Close
    [Authorize(ApplicationRole.Moderator)]
    [HttpPost]
    public async Task<IActionResult> Close(Guid? orderId)
    {
      if (!orderId.HasValue)
      {
        return NotFound();
      }

      var isSuccess = await _orderManager.CloseOrderAsync(orderId.Value);
      if (!isSuccess.HasValue)
      {
        return NotFound();
      }
      else if (isSuccess.Value)
      {
        this.StatusMessage = _localizer["Success: order has beed closed."];
      }
      else
      {
        this.StatusMessage = _localizer["Error: something has gone wrong while closing order."];
      }

      return RedirectToAction(nameof(OrderController.All));
    }

    // POST Order/Remove
    [HttpPost]
    public async Task<IActionResult> Remove(Guid? orderId)
    {
      if (!orderId.HasValue)
      {
        return NotFound();
      }

      var isSuccess = await _orderManager.RemoveOrderAsync(orderId.Value);
      if (!isSuccess.HasValue)
      {
        return NotFound();
      }
      else if (isSuccess.Value)
      {
        this.StatusMessage = _localizer["Success: order has beed remove."];
      }
      else
      {
        this.StatusMessage = _localizer["Error: something has gone wrong while removing order."];
      }

      return RedirectToAction(nameof(OrderController.All));
    }
  }
}