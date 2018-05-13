using System;
using System.ComponentModel.DataAnnotations;

namespace YourMotivation.Web.Models.TransferViewModels
{
  public enum FilterForTopUsers
  {
    ByDay,
    ByMonth,
    ByYear,
    AllTime
  }

  public class TopUser
  {
    public Guid Id { get; set; }

    [Display(Name = "Username")]
    public string Username { get; set; }

    [Display(Name = "Count")]
    public int Count { get; set; }
  }
}
