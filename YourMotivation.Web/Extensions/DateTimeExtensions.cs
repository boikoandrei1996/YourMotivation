using System;

namespace YourMotivation.Web.Extensions
{
  public static class DateTimeExtensions
  {
    public static string FormatDateTime(this DateTime dateTime, bool timeFirst = false)
    {
      if (timeFirst)
      {
        return dateTime.ToShortTimeString() + " || " + dateTime.ToShortDateString();
      }
      else
      {
        return dateTime.ToShortDateString() + " || " + dateTime.ToShortTimeString();
      }
    }
  }
}
