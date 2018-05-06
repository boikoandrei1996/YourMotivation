using System.Collections.Generic;
using YourMotivation.Web.Models.AdminViewModels;

namespace YourMotivation.Web.Models.Pagination.Pages
{
  public class AdminUsersPageViewModel : IBasePage<AdminUserViewModel>, ISortablePage<SortAdminViewModel>
  {
    public AdminUsersPageViewModel()
    {
      Records = new List<AdminUserViewModel>();
    }

    public List<AdminUserViewModel> Records { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage
    {
      get { return CurrentPage > 1; }
    }
    public bool HasNextPage
    {
      get { return CurrentPage < TotalPages; }
    }

    public SortAdminViewModel SortViewModel { get; set; }
    public string UsernameFilter { get; set; }

    public string StatusMessage { get; set; }
    public AdminUserViewModel Default { get { return new AdminUserViewModel(); } }
  }
}
