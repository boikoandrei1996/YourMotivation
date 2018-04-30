using System;
using System.Collections.Generic;

namespace ORM.Models
{
  public class Cart
  {
    public Guid Id { get; set; }

    public Guid UserOwnerId { get; set; }
    public ApplicationUser UserOwner { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public List<CartItem> CartItem { get; set; } = new List<CartItem>();
  }
}
