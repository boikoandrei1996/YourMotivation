using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ORM.Models
{
  public class ApplicationUser : IdentityUser<Guid>
  {
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    public int TotalPoints { get; set; }

    public int PointsPerMonth { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; }

    public List<Order> Orders { get; set; } = new List<Order>();
    public List<Transfer> TransfersAsSender { get; set; } = new List<Transfer>();
    public List<Transfer> TransfersAsReceiver { get; set; } = new List<Transfer>();
  }
}
