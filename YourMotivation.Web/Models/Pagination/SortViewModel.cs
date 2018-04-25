using System.ComponentModel;

namespace YourMotivation.Web.Models.Pagination
{
  public class SortViewModel
  {
    public SortState CurrentState { get; private set; }

    public SortViewModel(SortState sortOrder)
    {
      CurrentState = sortOrder;
    }

    public SortState NextState(SortColumnName columnName)
    {
      if (columnName == SortColumnName.Username)
      {
        switch (CurrentState)
        {
          case SortState.UsernameAsc:
            return SortState.UsernameDesc;
          case SortState.UsernameDesc:
            return SortState.UsernameAsc;
          default:
            return SortState.UsernameAsc;
        }
      }
      else if (columnName == SortColumnName.CreatedDate)
      {
        switch (CurrentState)
        {
          case SortState.CreatedDateAsc:
            return SortState.CreatedDateDesc;
          case SortState.CreatedDateDesc:
            return SortState.CreatedDateAsc;
          default:
            return SortState.CreatedDateDesc;
        }
      }

      throw new InvalidEnumArgumentException(nameof(SortColumnName));
    }

    public bool? IsUp(SortColumnName columnName)
    {
      if (columnName == SortColumnName.Username &&
        (CurrentState == SortState.UsernameAsc || CurrentState == SortState.UsernameDesc))
      {
        return CurrentState == SortState.UsernameAsc ? false : true;
      }
      else if (columnName == SortColumnName.CreatedDate &&
        (CurrentState == SortState.CreatedDateAsc || CurrentState == SortState.CreatedDateDesc))
      {
        return CurrentState == SortState.CreatedDateAsc ? false : true;
      }

      return null;
    }
  }
}
