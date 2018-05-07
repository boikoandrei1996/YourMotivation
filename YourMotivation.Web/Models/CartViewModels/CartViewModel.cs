using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ORM.Models;

namespace YourMotivation.Web.Models.CartViewModels
{
  public class CartViewModel
  {
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public List<CartItemViewModel> Items { get; set; }

    [Display(Name = "Count")]
    public int ItemsCount { get; set; }

    [Display(Name = "SumPrice")]
    public int ItemsSumPrice { get; set; }

    public string StatusMessage { get; set; }

    public CartItemViewModel DefaultItem { get { return new CartItemViewModel(); } }

    public static CartViewModel Map(Cart cart)
    {
      if (cart == null || !cart.UserId.HasValue)
      {
        return null;
      }

      return new CartViewModel
      {
        Id = cart.Id,
        UserId = cart.UserId.Value,
        ItemsCount = cart.CartItems.Sum(ci => ci.Count),
        ItemsSumPrice = cart.CartItems.Sum(ci => ci.Item.Price * ci.Count),
        Items = cart.CartItems.Select(ci => CartItemViewModel.Map(ci.Item, ci.Count)).ToList()
      };
    }
  }
}
