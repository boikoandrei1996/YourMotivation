using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.AccountViewModels
{
  public class NewTransferViewModel
  {
    [Required(ErrorMessage = ValidationMessages.RequiredReceiver)]
    [Display(Name = "UserReceiverUsername")]
    public string ReceiverUsername { get; set; }

    // [Required(ErrorMessage = ValidationMessages.RequiredPoints)]
    [Range(1, Constants.PointsRangeForNewTransfer, ErrorMessage = ValidationMessages.RangeInvalid)]
    [Display(Name = "Points")]
    public int Points { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
    [Display(Name = "Message")]
    public string Message { get; set; }

    public string FormErrorMessage { get; set; }
  }
}
