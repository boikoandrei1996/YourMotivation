using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Models.TransferViewModels
{
  public class ShowTransferViewModel
  {
    public Guid Id { get; set; }

    [Display(Name = "Message")]
    public string Message { get; set; }

    [Display(Name = "TransferDateTime")]
    public string TransferDateTime { get; set; }

    [Display(Name = "Points")]
    public int Points { get; set; }

    public Guid UserSenderId { get; set; }

    [Display(Name = "UserSenderUsername")]
    public string UserSenderUsername { get; set; }

    public Guid UserReceiverId { get; set; }

    [Display(Name = "UserReceiverUsername")]
    public string UserReceiverUsername { get; set; }

    public static ShowTransferViewModel Map(Transfer transfer)
    {
      if (transfer == null)
      {
        return null;
      }

      return new ShowTransferViewModel
      {
        Id = transfer.Id,
        Message = transfer.Text,
        Points = transfer.Points,
        TransferDateTime =transfer.DateOfCreation.FormatDateTime(timeFirst: true),
        UserSenderId = transfer.UserSenderId,
        UserSenderUsername = transfer.UserSender.UserName,
        UserReceiverId = transfer.UserReceiverId,
        UserReceiverUsername = transfer.UserReceiver.UserName
      };
    }
  }
}
