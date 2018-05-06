using System.ComponentModel;

namespace YourMotivation.Web.Models.Pagination
{
  public class SortOrderViewModel
  {
    public SortState CurrentState { get; private set; }

    public SortOrderViewModel(SortState sortOrder)
    {
      CurrentState = sortOrder;
    }

    public SortState NextState(SortColumnName columnName)
    {
      if (columnName == SortColumnName.DateOfClosing)
      {
        switch (CurrentState)
        {
          case SortState.DateOfClosingAsc:
            return SortState.DateOfClosingDesc;
          case SortState.DateOfClosingDesc:
            return SortState.DateOfClosingAsc;
          default:
            return SortState.DateOfClosingDesc;
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
      if (columnName == SortColumnName.DateOfClosing &&
        (CurrentState == SortState.DateOfClosingAsc || CurrentState == SortState.DateOfClosingDesc))
      {
        return CurrentState == SortState.DateOfClosingAsc ? false : true;
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
