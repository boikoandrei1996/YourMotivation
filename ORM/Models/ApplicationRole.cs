using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ORM.Models
{
  public class ApplicationRole : IdentityRole<Guid>
  {
    public const string Admin = "Admin";
    public const string Moderator = "Moderator";
    public const string User = "User";

    public static IReadOnlyList<string> GetAllRoles()
    {
      return new List<string>
      {
        Admin,
        Moderator,
        User
      };
    }
  }
}
