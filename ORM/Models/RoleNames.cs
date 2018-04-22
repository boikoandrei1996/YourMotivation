using System.Collections.Generic;

namespace ORM.Models
{
  public class RoleNames
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
