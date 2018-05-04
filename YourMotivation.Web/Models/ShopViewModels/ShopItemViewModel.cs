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
        Title = model.Title ?? string.Empty,
        Description = model.Characteristics.Description ?? string.Empty,
        Price = model.Price,
        IsInStock = model.CountsInStock > 0,
        Size = string.IsNullOrEmpty(model.Characteristics.Size) ? "None" : model.Characteristics.Size,
        Color = string.IsNullOrEmpty(model.Characteristics.Color) ? "None" : model.Characteristics.Color,
        Model = string.IsNullOrEmpty(model.Characteristics.Model) ? "None" : model.Characteristics.Model
      };
    }
  }
}
