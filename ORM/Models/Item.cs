using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ORM.Models
{
  public class Item
  {
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public byte[] Image { get; set; }

    public int Price { get; set; }

    public int CountsInStock { get; set; }

    public ItemCharacteristics Characteristics { get; set; }

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
  }
}
