namespace YourMotivation.Web.Extensions
{
  public static class StringExtensions
  {
    public static string GetUsernameFromEmail(this string email)
    {
      return email.Split('@')[0];
    }

    public static string Truncate(this string @this, int length)
    {
      if (@this.Length < length)
      {
        return @this;
      }

      return @this.Substring(0, length);
    }
  }
}
