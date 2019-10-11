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
      if (packageTree.Children.Any())
      {
        return Tag("ul", packageTree.Children.OrderByDescending(c => c.HotSpotRating)
          .Select(childPackage => Tag("li", RenderChildPackage(childPackage))).ToArray());
      }
      else
      {
        return string.Empty;
      }
    }

    private static string RenderChildPackage(PackageTreeNodeViewModel childPackage)
    {
      if (childPackage.Children.Any())
      {
        return
          Tag("details",
            Tag("summary", childPackage.Name + " (" + childPackage.HotSpotRating + ")"),
            RenderFrom(childPackage));
      }
      else
      {
        return Tag("span", childPackage.Name + " (" + childPackage.HotSpotRating + ")");
      }
    }

    private static string Tag(string tagName, params string[] inside)
    {
      return $"<{tagName}>" + string.Join(" ", inside) + $"</{tagName}>";
    }
  }

}