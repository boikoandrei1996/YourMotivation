using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ORM.Models;
using YourMotivation.Web.Models.AdminUsersViewModels;
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.AdminUsers;

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

    public static async Task<AdminUsersPageViewModel> GetUserPageAsync(
      this UserManager<ApplicationUser> userManager, int index,
      int pageSize, string usernameFilter, SortState sortState)
    {
      var query = userManager.Users.AsQueryable();

      if (!string.IsNullOrWhiteSpace(usernameFilter))
      {
        query = query.Where(user => user.UserName.Contains(usernameFilter));
      }

      switch (sortState)
      {
        case SortState.UsernameAsc:
          query = query.OrderBy(user => user.UserName);
          break;
        case SortState.UsernameDesc:
          query = query.OrderByDescending(user => user.UserName);
          break;
        case SortState.CreatedDateAsc:
          query = query.OrderBy(user => user.CreatedDate);
          break;
        case SortState.CreatedDateDesc:
          query = query.OrderByDescending(user => user.CreatedDate);
          break;
      }

      var result = new AdminUsersPageViewModel
      {
        CurrentPage = index,
        PageSize = pageSize,
        UsernameFilter = usernameFilter,
        SortViewModel = new SortViewModel(sortState)
      };

      var totalCount = await query.CountAsync();
      result.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

      query = query
        .Skip((index - 1) * pageSize)
        .Take(pageSize);

      var users = new List<AdminUserViewModel>();
      foreach (var user in await query.ToListAsync())
      {
        var role = await userManager.GetUserRoleAsync(user);
        users.Add(AdminUserViewModel.Map(user, role));
      }

      result.Records = users;

      return result;
    }
  }
}
