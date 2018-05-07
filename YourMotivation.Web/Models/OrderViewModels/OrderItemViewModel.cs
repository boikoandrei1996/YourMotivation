using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;

namespace YourMotivation.Web.Models.OrderViewModels
{
  public class OrderItemViewModel
  {
    [Display(Name = "Id")]
    public Guid Id { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "IsInStock")]
    public bool IsInStock { get; set; }

    [Display(Name = "CountInCart")]
    public int Count { get; set; }

    public static OrderItemViewModel Map(Item item, int count)
    {
      if (item == null)
      {
        return null;
      }

      return new OrderItemViewModel
      {
        Id = item.Id,
        Title = item.Title,
        IsInStock = item.CountsInStock - count >= 0,
        Count = count
      };
    }
  }
}
