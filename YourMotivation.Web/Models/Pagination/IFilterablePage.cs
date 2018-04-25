namespace YourMotivation.Web.Models.Pagination
{
  public interface IFilterablePage
  {
    string UsernameFilter { get; set; }
    string RoleFilter { get; set; }
  }
}
