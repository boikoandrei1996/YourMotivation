using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.AccountViewModels
{
  public class ForgotPasswordViewModel
  {
    [Required(ErrorMessage = ValidationMessages.RequiredEmail)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailAddressInvalid)]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }
}
