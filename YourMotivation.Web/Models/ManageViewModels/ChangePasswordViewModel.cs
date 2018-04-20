using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.ManageViewModels
{
  public class ChangePasswordViewModel
  {
    [Required(ErrorMessage = ValidationMessages.RequiredPassword)]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredPassword)]
    [StringLength(100, ErrorMessage = ValidationMessages.StringLengthRestriction, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = ValidationMessages.ComparePasswordInvalid)]
    [Display(Name = "Confirm new password")]
    public string ConfirmPassword { get; set; }

    public string StatusMessage { get; set; }
  }
}
