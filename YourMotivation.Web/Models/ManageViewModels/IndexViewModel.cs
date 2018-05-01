using System.ComponentModel.DataAnnotations;
using ORM.Models;

namespace YourMotivation.Web.Models.ManageViewModels
{
  public class IndexViewModel
  {
    [Display(Name = "Username")]
    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }
    
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Phone(ErrorMessage = ValidationMessages.PhoneInvalid)]
    [Display(Name = "PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Display(Name = "TotalPoints")]
    public int TotalPoints { get; set; }

    [Display(Name = "PointsPerMonth")]
    public int PointsPerMonth { get; set; }

    public string StatusMessage { get; set; }

    public static IndexViewModel Map(ApplicationUser user, string statusMessage)
    {
      return new IndexViewModel
      {
        Username = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        IsEmailConfirmed = user.EmailConfirmed,
        TotalPoints = user.TotalPoints,
        PointsPerMonth = user.PointsPerMonth,
        StatusMessage = statusMessage
      };
    }
  }
}
