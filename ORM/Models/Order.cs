using System;
using System.ComponentModel.DataAnnotations;

namespace ORM.Models
{
  public class Order
  {
    public Guid Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime DateOfCreation { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DateOfClosing { get; set; }
    
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; }
  }
}
