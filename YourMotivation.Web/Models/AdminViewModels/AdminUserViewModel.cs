using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;

namespace YourMotivation.Web.Models.AdminViewModels
{
  public class AdminUserViewModel
  {
    public string Id { get; set; }

    [Display(Name = "Username")]
    public string Username { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Role")]
    public string Role { get; set; }

    [Display(Name = "CreatedDate")]
    public DateTime CreatedDate { get; set; }

    public string StatusMessage { get; set; }

    public static AdminUserViewModel Map(ApplicationUser user, string role)
    {
      return new AdminUserViewModel
      {
        Id = user.Id.ToString(),
        Username = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Role = role,
        CreatedDate = user.CreatedDate
      };
    }
  }
}
