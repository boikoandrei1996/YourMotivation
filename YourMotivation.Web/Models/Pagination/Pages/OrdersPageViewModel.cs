using System.Collections.Generic;
using YourMotivation.Web.Models.OrderViewModels;

namespace YourMotivation.Web.Models.Pagination.Pages
{
  public class OrdersPageViewModel : IBasePage<ShowOrderViewModel>, ISortablePage<SortOrderViewModel>
  {
    public OrdersPageViewModel()
    {
      Records = new List<ShowOrderViewModel>();
    }

    public List<ShowOrderViewModel> Records { get; set; }
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

    public SortOrderViewModel SortViewModel { get; set; }

    public string StatusMessage { get; set; }
    public ShowOrderViewModel Default { get { return new ShowOrderViewModel(); } }
  }
}
