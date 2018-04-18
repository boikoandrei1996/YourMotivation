using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.AccountViewModels
{
  public class ForgotPasswordViewModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
