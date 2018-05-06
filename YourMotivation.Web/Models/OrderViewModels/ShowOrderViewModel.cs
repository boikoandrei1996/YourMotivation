using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Models.OrderViewModels
{
  public class ShowOrderViewModel
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }

    [Display(Name = "OwnerUsername")]
    public string OwnerUsername { get; set; }

    [Display(Name = "DateOfCreation")]
    public string DateOfCreation { get; set; }

    [Display(Name = "DateOfClosing")]
    public string DateOfClosing { get; set; }

    [Display(Name = "IsClosed")]
    public bool IsClosed { get; set; }

    public static ShowOrderViewModel Map(Order order)
    {
      if (order == null)
      {
        return null;
      }

      return new ShowOrderViewModel
      {
        Id = order.Id,
        UserId = order.UserId,
        CartId = order.CartId,
        OwnerUsername = order.User.UserName,
        DateOfCreation = order.DateOfCreation.FormatDateTime(),
        DateOfClosing =
          order.DateOfClosing.HasValue ?
          order.DateOfClosing.Value.FormatDateTime() :
          "None",
        IsClosed = order.DateOfClosing.HasValue
      };
    }
  }
}
