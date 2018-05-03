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

    [DataType(DataType.DateTime)]
    public DateTime DateOfCreation { get; set; }

    public int Points { get; set; }

    public Guid UserSenderId { get; set; }
    public ApplicationUser UserSender { get; set; }
    
    public Guid UserReceiverId { get; set; }
    public ApplicationUser UserReceiver { get; set; }
  }
}
