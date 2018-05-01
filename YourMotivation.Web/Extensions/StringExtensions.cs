namespace YourMotivation.Web.Extensions
{
  public static class StringExtensions
  {
    public static string GetUsernameFromEmail(this string email)
    {
      return email.Split('@')[0];
    }
  }
}
