﻿using System;
using System.ComponentModel.DataAnnotations;
using ORM.Models;
using YourMotivation.Web.Extensions;

namespace YourMotivation.Web.Models.AdminViewModels
{
  public class AdminUserViewModel
  {
    public Guid Id { get; set; }

    [Display(Name = "Username")]
    public string Username { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Role")]
    public string Role { get; set; }

    [Display(Name = "CreatedDate")]
    public string CreatedDate { get; set; }

    public string StatusMessage { get; set; }

    public static AdminUserViewModel Map(ApplicationUser user, string role)
    {
      if (user == null)
      {
        return null;
      }

      return new AdminUserViewModel
      {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Role = role,
        CreatedDate = user.CreatedDate.FormatDateTime(" || ", timeFirst: true)
      };
    }
  }
}
