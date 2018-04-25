using System.Collections.Generic;

namespace YourMotivation.Web.Models.Pagination
{
  public interface IBasePage<T>
  {
    List<T> Records { get; set; }
    int CurrentPage { get; set; }
    int PageSize { get; set; }
    int TotalPages { get; set; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
  }
}
