using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORM;
using ORM.Models;

namespace YourMotivation.Web.Extensions
{
  public static class DbContextUsersExtensions
  {
    public async static Task<ApplicationUser> FindUserByIdAsync(
      this ApplicationDbContext context,
      Guid id,
      bool isTrackable = true)
    {
      var users = context.Users
        .Include(e => e.TransfersAsSender)
        .Include(e => e.TransfersAsReceiver);

      if (isTrackable)
      {
        return await users.FirstOrDefaultAsync(u => u.Id == id);
      }
      else
      {
        return await users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
      }
    }
  }
}
