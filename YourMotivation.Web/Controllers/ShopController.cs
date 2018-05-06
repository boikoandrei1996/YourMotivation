﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ORM.Models;
using YourMotivation.Web.Services;

namespace YourMotivation.Web.Controllers
{
  [Route("[controller]/item/[action]")]
  public class ShopController : Controller
  {
    private readonly ShopManager _shopManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<ShopController> _localizer;

    public ShopController(
      ShopManager shopManager,
      ILogger<ShopController> logger,
      IStringLocalizer<ShopController> localizer)
    {
      _shopManager = shopManager;
      _logger = logger;
      _localizer = localizer;
    }

    [TempData]
    public string StatusMessage { get; set; }

    // GET: Shop/Item/All
    [HttpGet]
    public async Task<IActionResult> All(int? pageIndex, string titleFilter)
    {
      var page =
        await _shopManager.GetShopItemPageAsync(pageIndex ?? 1, 2, titleFilter);

      page.StatusMessage = this.StatusMessage;

      return View(page);
    }

    // GET: Shop/Item/Image
    [ActionName("Image")]
    public async Task<IActionResult> GetImage(Guid? itemId)
    {
      if (!itemId.HasValue)
      {
        return NotFound();
      }

      var imageTuple = await _shopManager.GetItemImageAsync(itemId.Value);
      if (imageTuple.Content == null)
      {
        return NotFound();
      }

      return File(imageTuple.Content, imageTuple.ContentMimeType);
    }

    // GET: Shop/Item/Cart/id
    [ActionName("Cart")]
    public async Task<IActionResult> ShowCart(Guid? id)
    {
      if (!id.HasValue)
      {
        return NotFound();
      }

      var cart = await _shopManager.FindCartByIdAsync(id.Value);
      if (cart == null)
      {
        return NotFound();
      }

      return View(nameof(ShowCart), cart);
    }

    /*// GET: Shop/Item/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Shop/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Image,ImageContentType,Price,CountsInStock")] Item item)
    {
      if (ModelState.IsValid)
      {
        item.Id = Guid.NewGuid();
        _context.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(item);
    }

    // GET: Shop/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      return View(item);
    }

    // POST: Shop/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Image,ImageContentType,Price,CountsInStock")] Item item)
    {
      if (id != item.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(item);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ItemExists(item.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(item);
    }

    // GET: Shop/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var item = await _context.Items
          .SingleOrDefaultAsync(m => m.Id == id);
      if (item == null)
      {
        return NotFound();
      }

      return View(item);
    }

    // POST: Shop/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
      var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
      _context.Items.Remove(item);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }*/
  }
}
