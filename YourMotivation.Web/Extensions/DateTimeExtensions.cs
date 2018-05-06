using System;
using System.Collections.Generic;
using System.Linq;

namespace YourMotivation.Web.Extensions
{
  public static class DateTimeExtensions
  {
    public static string FormatDateTime(this DateTime dateTime)
    {
      return dateTime.ToShortDateString() + " || " + dateTime.ToShortTimeString();
    }
  }
}
