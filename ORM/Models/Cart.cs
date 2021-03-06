﻿using System;
using System.Collections.Generic;

namespace ORM.Models
{
  public class Cart
  {
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Order Order { get; set; }

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
  }
}
