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
    public static string GetJavaScript() => 
      @"";

    public static string RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      string result = string.Empty;

      if (packageTree.Children.Any())
      {
        result += "<ul>";
        foreach (var childPackage in packageTree.Children.OrderByDescending(c => c.HotSpotRating))
        {
          result += "<li>";
          if (childPackage.Children.Any())
          {
            result += "<details>" + "<summary>" + childPackage.Name + " (" + childPackage.HotSpotRating + ")" + "</summary>";
            result += RenderFrom(childPackage);
            result += "</details>";
          }
          else
          {
            result += $"<span>" + childPackage.Name + " (" + childPackage.HotSpotRating + ")" + "</span>";
          }
          result += "</li>";
        }

        result += "</ul>";
      }

      return result;
    }
  }
}
