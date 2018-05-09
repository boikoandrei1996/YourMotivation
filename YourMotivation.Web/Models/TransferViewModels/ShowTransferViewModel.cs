using System.ComponentModel.DataAnnotations;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Models.TransferViewModels
{
  public class ShowTransferViewModel
  {
    [Display(Name = "Message")]
    public string Message { get; set; }

    [Display(Name = "TransferDateTime")]
    public string TransferDateTime { get; set; }

    [Display(Name = "Points")]
    public int Points { get; set; }

    [Display(Name = "UserSenderUsername")]
    public string UserSenderUsername { get; set; }

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
        Message = transfer.Text,
        Points = transfer.Points,
        TransferDateTime =transfer.DateOfCreation.FormatDateTime(timeFirst: true),
        UserSenderUsername = transfer.UserSender.UserName,
        UserReceiverUsername = transfer.UserReceiver.UserName
      };
    }
  }
}
