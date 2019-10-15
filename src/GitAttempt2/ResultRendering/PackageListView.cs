using System.Linq;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class PackageListView
  {
    public static IHtmlContent RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      return Tag("div", H(1, "Hot Spot rating per package(nested)"), RenderChildFrom(packageTree));
    }

    private static IHtmlContent RenderChildFrom(PackageTreeNodeViewModel packageTree)
    {
      if (packageTree.Children.Any())
      {
        return Tag("div", Tag("ul", packageTree.Children.OrderByDescending(c => c.HotSpotRating)
          .Select(childPackage => Tag("li", RenderChildPackage(childPackage))).ToArray()));
      }
      else
      {
        return VerbatimText("");
      }
    }

    private static IHtmlContent RenderChildPackage(PackageTreeNodeViewModel childPackage)
    {
      if (childPackage.Children.Any())
      {
        return Tag("details", Tag("summary", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")")),
          RenderChildFrom(childPackage));
      }
      else
      {
        return Tag("span", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")"));
      }
    }
  }
}