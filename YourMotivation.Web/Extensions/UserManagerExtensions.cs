using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ORM.Models;
using YourMotivation.Web.Models;
using YourMotivation.Web.Models.AdminUsersViewModels;

namespace YourMotivation.Web.Extensions
{
  public static class UserManagerExtensions
  {
    public static async Task<IdentityResult> DeleteUserAsync<T>(this UserManager<T> userManager, T user)
      where T : class
    {
      try
      {
        return await userManager.DeleteAsync(user);
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(userManager.DeleteAsync),
          Description = ex.InnerException.InnerException.Message
        });
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static async Task<IdentityResult> CreateUserAsync<T>(
      this UserManager<T> userManager, T user, string password)
      where T : class
    {
      var result = await userManager.CreateAsync(user, password);
      if (!result.Succeeded)
      {
        return result;
      }

      result = await userManager.AddToRoleAsync(user, RoleNames.User);
      if (!result.Succeeded)
      {
        await userManager.DeleteAsync(user);
      }

      return result;
    }

    public static async Task<string> GetUserRoleAsync<T>(this UserManager<T> userManager, T user)
      where T : class
    {
      var roles = await userManager.GetRolesAsync(user);

      return roles.SingleOrDefault();
    }

    public static async Task<Page<AdminUserViewModel>> GetUserPageAsync<T>(
      this UserManager<T> userManager, int index,
      int pageSize, string username,
      string sortColumn = null, bool orderBy = false)
      where T : ApplicationUser
    {
      var result = new Page<AdminUserViewModel>
      {
        CurrentPage = index,
        PageSize = pageSize
      };

      // TODO: Optimization.
      var users = new List<AdminUserViewModel>();
      foreach (var user in userManager.Users)
      {
        var role = await userManager.GetUserRoleAsync(user);
        users.Add(AdminUserViewModel.Map(user, role));
      }

      var query = users.AsQueryable();
      if (!string.IsNullOrWhiteSpace(username))
      {
        query = query.Where(user => user.Username.Contains(username));
      }

      if (!string.IsNullOrWhiteSpace(sortColumn))
      {
        switch (sortColumn)
        {
          case SortColumnOptions.Username:
            query = orderBy ? 
              query.OrderBy(user => user.Username) : 
              query.OrderByDescending(user => user.Username);
            break;
          case SortColumnOptions.CreatedDate:
            query = orderBy ?
              query.OrderBy(user => user.CreatedDate) :
              query.OrderByDescending(user => user.CreatedDate);
            break;
        }
      }
      else
      {
        query = query.OrderByDescending(user => user.CreatedDate);
      }

      var count = await query.CountAsyncSafe();
      result.TotalPages = count % pageSize == 0 ?
        count / pageSize :
        count / pageSize + 1;

      query = query
        .Skip((index - 1) * pageSize)
        .Take(pageSize);

      result.Records = await query.ToListAsyncSafe();

      return result;
    }
  }
}
