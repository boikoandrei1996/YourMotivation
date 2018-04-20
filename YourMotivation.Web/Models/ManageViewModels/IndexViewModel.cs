using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.ManageViewModels
{
  public class IndexViewModel
  {
    [Display(Name = "Username")]
    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredEmail)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailAddressInvalid)]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Phone(ErrorMessage = ValidationMessages.PhoneInvalid)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    public string StatusMessage { get; set; }
  }
}
