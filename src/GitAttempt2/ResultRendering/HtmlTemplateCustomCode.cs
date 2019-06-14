using System.Linq;

namespace ResultRendering
{
  public partial class HtmlTemplate
  {
    public HtmlTemplate(ViewModel viewModel)
    {
      _viewModel = viewModel;
    }

  }

  public class PackageListView
  {
    public static string RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      string result = string.Empty;

      if (packageTree.Children.Any())
      {
        result += "<ul style=\"list-style-type:none\" >";
        foreach (var childPackage in packageTree.Children.OrderByDescending(c => c.HotSpotRating))
        {
          result += "<li>";
          result += "<span>" + childPackage.Name + " (" + childPackage.HotSpotRating + ")" + "</span>";
          result += RenderFrom(childPackage);
          result += "</li>";
        }
        result += "</ul>";
      }

      return result;
    }
  }
}
