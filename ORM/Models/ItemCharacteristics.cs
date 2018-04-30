using System;
using System.ComponentModel.DataAnnotations;

namespace ORM.Models
{
  public class ItemCharacteristics
  {
    public Guid Id { get; set; }

    [DataType(DataType.Text)]
    public string Description { get; set; }

    [MaxLength(25)]
    public string Size { get; set; }

    [MaxLength(25)]
    public string Color { get; set; }

    [MaxLength(25)]
    public string Model { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }
  }
}
