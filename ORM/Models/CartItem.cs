using System;

namespace ORM.Models
{
  public class CartItem
  {
    public CartItem()
    {
      Count = 1;
    }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }

    public int Count { get; set; }
  }
}
