using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.AccountViewModels
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = ValidationMessages.RequiredEmail)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailAddressInvalid)]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredPassword)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
  }
}
