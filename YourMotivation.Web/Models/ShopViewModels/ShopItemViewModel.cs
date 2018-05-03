using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;

namespace YourMotivation.Web.Models.ShopViewModels
{
  public class ShopItemViewModel
  {
    public Guid Id { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    [Display(Name = "Price")]
    public int Price { get; set; }

    [Display(Name = "IsInStock")]
    public bool IsInStock { get; set; }

    [Display(Name = "Size")]
    public string Size { get; set; }

    [Display(Name = "Color")]
    public string Color { get; set; }

    [Display(Name = "Model")]
    public string Model { get; set; }

    public static ShopItemViewModel Map(Item model)
    {
      if (model == null)
      {
        return null;
      }

      return new ShopItemViewModel
      {
        Id = model.Id,
        Title = model.Title,
        Description = model.Characteristics.Description,
        Price = model.Price,
        IsInStock = model.CountsInStock > 0,
        Size = model.Characteristics.Size,
        Color = model.Characteristics.Color,
        Model = model.Characteristics.Model
      };
    }
  }
}
