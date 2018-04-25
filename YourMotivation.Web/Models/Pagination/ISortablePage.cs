namespace YourMotivation.Web.Models.Pagination
{
  public interface ISortablePage<T>
  {
    T SortViewModel { get; set; }
  }
}
