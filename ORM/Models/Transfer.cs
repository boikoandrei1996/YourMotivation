using System;
using System.ComponentModel.DataAnnotations;

namespace ORM.Models
{
  public class Transfer
  {
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string Text { get; set; }

    public int Points { get; set; }

    public Guid UserSenderId { get; set; }
    
    public Guid UserReceiverId { get; set; }
  }
}
