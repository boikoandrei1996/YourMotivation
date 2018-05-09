using System;

namespace YourMotivation.Web.Extensions
{
  public static class DateTimeExtensions
  {
    public static string FormatDateTime(this DateTime dateTime, string separator, bool timeFirst = false)
    {
      if (timeFirst)
      {
        return dateTime.ToShortTimeString() + separator + dateTime.ToShortDateString();
      }
      else
      {
        return dateTime.ToShortDateString() + separator + dateTime.ToShortTimeString();
      }
    }
  }
}
