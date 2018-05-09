using System.Collections.Generic;
using YourMotivation.Web.Models.AccountViewModels;
using YourMotivation.Web.Models.TransferViewModels;

namespace YourMotivation.Web.Models.Pagination.Pages
{
  public class TransfersPageViewModel : IBasePage<ShowTransferViewModel>
  {
    public TransfersPageViewModel()
    {
      Records = new List<ShowTransferViewModel>();
    }

    public List<ShowTransferViewModel> Records { get; set; }
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

    public string StatusMessage { get; set; }
    public NewTransferViewModel NewTransferModel { get; set; }
    public ShowTransferViewModel Default { get { return new ShowTransferViewModel(); } }
  }
}
