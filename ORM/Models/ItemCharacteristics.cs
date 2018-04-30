using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORM.Models
{
  public class ItemCharacteristics
  {
    [DataType(DataType.Text)]
    public string Description { get; set; }

    [MaxLength(25)]
    public string Size { get; set; }

    [MaxLength(25)]
    public string Color { get; set; }

    [MaxLength(25)]
    public string Model { get; set; }

    [Key, ForeignKey("Item")]
    public Guid Id { get; set; }
    public Item Item { get; set; }
  }
}
