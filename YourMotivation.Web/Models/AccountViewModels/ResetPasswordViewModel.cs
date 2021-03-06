﻿using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.AccountViewModels
{
  public class ResetPasswordViewModel
  {
    [Required(ErrorMessage = ValidationMessages.RequiredEmail)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailAddressInvalid)]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredPassword)]
    [StringLength(100, ErrorMessage = ValidationMessages.StringLengthRestriction, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ValidationMessages.ComparePasswordInvalid)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
  }
}
