using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Models.CartViewModels
{
  public class CartItemViewModel
  {
    public Guid Id { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "Price")]
    public int Price { get; set; }

    [Display(Name = "IsInStock")]
    public bool IsInStock { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    [Display(Name = "Size")]
    public string Size { get; set; }

    [Display(Name = "Color")]
    public string Color { get; set; }

    [Display(Name = "Model")]
    public string Model { get; set; }

    public static CartItemViewModel Map(Item item)
    {
      if (item == null)
      {
        return null;
      }

      return new CartItemViewModel
      {
        Id = item.Id,
        Title = item.Title,
        Description = item.Characteristics.Description.Truncate(Constants.ItemPreviewDescriptionMaxLength),
        Price = item.Price,
        IsInStock = item.CountsInStock > 0,
        Color = item.Characteristics.Color,
        Size = item.Characteristics.Size,
        Model = item.Characteristics.Model
      };
    }
  }
}
