using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ORM.Models;

namespace YourMotivation.Web.Models.CartViewModels
{
  public class CartPreviewViewModel
  {
    public Guid Id { get; set; }

    [Display(Name = "Count")]
    public int ItemCount { get; set; }

    public static CartPreviewViewModel Map(Cart cart)
    {
      if (cart == null)
      {
        return null;
      }

      return new CartPreviewViewModel
      {
        Id = cart.Id,
        ItemCount = cart.CartItems.Sum(ci => ci.Count)
      };
    }
  }
}
