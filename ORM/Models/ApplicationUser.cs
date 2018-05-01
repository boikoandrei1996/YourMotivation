using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ORM.Models
{
  public class ApplicationUser : IdentityUser<Guid>
  {
    public ApplicationUser()
    {
      CreatedDate = DateTime.UtcNow;
      TotalPoints = 0;
      PointsPerMonth = 5;
      Cart = new Cart();
    }

    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    public int TotalPoints { get; set; }

    public int PointsPerMonth { get; set; }

    public Cart Cart { get; set; }

    // public List<Order> Orders { get; set; } = new List<Order>();
  }
}
