using System.Linq;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class PackageListView
  {
    public static IHtmlContent RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      if (packageTree.Children.Any())
      {
        return Tag("ul", packageTree.Children.OrderByDescending(c => c.HotSpotRating)
          .Select(childPackage => Tag("li", RenderChildPackage(childPackage))).ToArray());
      }
      else
      {
        return Text(string.Empty);
      }
    }

    private static IHtmlContent RenderChildPackage(PackageTreeNodeViewModel childPackage)
    {
      if (childPackage.Children.Any())
      {
        return Tag("details", Tag("summary", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")")), RenderFrom(childPackage));
      }
      else
      {
        return Tag("span", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")"));
      }
    }
  }
}