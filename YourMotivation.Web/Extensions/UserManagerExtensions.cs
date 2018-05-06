using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ORM;
using ORM.Models;
using YourMotivation.Web.Models.AdminViewModels;
using YourMotivation.Web.Models.CartViewModels;
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.Pages;

namespace YourMotivation.Web.Extensions
{
  public static class UserManagerExtensions
  {
    public static async Task<IdentityResult> DeleteUserAsync(
      this UserManager<ApplicationUser> userManager,
      ApplicationDbContext context,
      ApplicationUser user)
    {
      try
      {
        context.Transfers.RemoveRange(user.TransfersAsSender);
        context.Transfers.RemoveRange(user.TransfersAsReceiver);
        await context.SaveChangesAsync();

        return await userManager.DeleteAsync(user);
      }
      catch (DbUpdateException ex)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = nameof(userManager.DeleteAsync),
          Description = ex.InnerException.Message
        });
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static async Task<IdentityResult> CreateUserWithRoleAsync(
      this UserManager<ApplicationUser> userManager,
      ApplicationDbContext context,
      ApplicationUser user, 
      string password)
    {
      var result = await userManager.CreateAsync(user, password);
      if (!result.Succeeded)
      {
        return result;
      }

      result = await userManager.AddToRoleAsync(user, ApplicationRole.User);
      if (!result.Succeeded)
      {
        await userManager.DeleteUserAsync(context, user);
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
      this UserManager<ApplicationUser> userManager, 
      int index,
      int pageSize, 
      string usernameFilter, 
      SortState sortState)
    {
      var query = userManager.Users.AsNoTracking();

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
        SortViewModel = new SortAdminViewModel(sortState)
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

    public static async Task<CartPreviewViewModel> GetShopCartPreviewAsync(
      this UserManager<ApplicationUser> userManager, 
      string username)
    {
      var user = await userManager.Users
        .AsNoTracking()
        .Include(u => u.Cart)
          .ThenInclude(c => c.CartItems)
        .FirstOrDefaultAsync(u => u.UserName == username);

      if (user == null)
      {
        return null;
      }

      return CartPreviewViewModel.Map(user.Cart);
    }

    public static async Task<Guid> GetUserCartIdAsync(
      this UserManager<ApplicationUser> userManager,
      ClaimsPrincipal principal)
    {
      var username = userManager.GetUserName(principal);

      var user = await userManager.Users
        .AsNoTracking()
        .Include(u => u.Cart)
        .FirstAsync(u => u.UserName == username);

      return user.Cart.Id;
    }
  }
}
