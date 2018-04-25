using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using YourMotivation.Web.Models.Pagination;
using YourMotivation.Web.Models.Pagination.AdminUsers;

namespace YourMotivation.Web.TagHelpers
{
  public class AdminUsersSortHeaderTagHelper : TagHelper
  {
    private IUrlHelperFactory _urlHelperFactory;

    public AdminUsersSortHeaderTagHelper(IUrlHelperFactory helperFactory)
    {
      _urlHelperFactory = helperFactory;
    }

    public string Action { get; set; }
    public string Controller { get; set; }
    public AdminUsersPageViewModel PageModel { get; set; }
    public SortColumnName Column { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      var routeData = new
      {
        pageIndex = PageModel.CurrentPage,
        usernameFilter = PageModel.UsernameFilter,
        sortOrder = PageModel.SortViewModel.NextState(Column)
      };

      IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
      string url = string.IsNullOrEmpty(Controller) ?
        urlHelper.Action(Action, routeData) :
        urlHelper.Action(Action, Controller, routeData);

      output.TagName = "a";
      output.Attributes.SetAttribute("href", url);

      bool? up = PageModel.SortViewModel.IsUp(Column);
      if (up.HasValue)
      {
        var tag = new TagBuilder("i");
        tag.AddCssClass("glyphicon");

        if (up.Value)
        {
          tag.AddCssClass("glyphicon-chevron-up");
        }
        else
        {
          tag.AddCssClass("glyphicon-chevron-down");
        }

        output.PreContent.AppendHtml(tag);
      }
    }
  }
}
