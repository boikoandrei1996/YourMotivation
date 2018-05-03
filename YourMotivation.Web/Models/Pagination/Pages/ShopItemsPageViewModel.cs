using System.Collections.Generic;
using YourMotivation.Web.Models.ShopViewModels;

namespace YourMotivation.Web.Models.Pagination.Pages
{
  public class ShopItemsPageViewModel : IBasePage<ShopItemViewModel>
  {
    public ShopItemsPageViewModel()
    {
      Records = new List<ShopItemViewModel>();
    }

    public List<ShopItemViewModel> Records { get; set; }
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

    public string TitleFilter { get; set; }

    public string StatusMessage { get; set; }
    public ShopItemViewModel Default { get { return new ShopItemViewModel(); } }
  }
}
