using System.Collections.Generic;

namespace YourMotivation.Web.Models.Pagination
{
  public class Page<T> : IBasePage<T>, ISortablePage<SortViewModel>, IFilterablePage 
    where T : class, new()
  {
    public Page()
    {
      Records = new List<T>();
    }

    public List<T> Records { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public SortViewModel SortViewModel { get; set; }

    public string UsernameFilter { get; set; }

    public string StatusMessage { get; set; }
    public T Default { get { return new T(); } }
  }
}
