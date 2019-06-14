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
      @"var toggler = document.getElementsByClassName(""caret"");
var i;

for (i = 0; i < toggler.length; i++) {
  toggler[i].addEventListener(""click"", function() {
    this.parentElement.querySelector("".nested"").classList.toggle(""active"");
    this.classList.toggle(""caret-down"");
  });
}";

    public static string RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      return Render2(packageTree, "id=\"myUL\"");
    }

    private static string Render2(PackageTreeNodeViewModel packageTree, string ulAttribute)
    {
      string result = string.Empty;

      if (packageTree.Children.Any())
      {
        result += $"<ul {ulAttribute}>";
        foreach (var childPackage in packageTree.Children.OrderByDescending(c => c.HotSpotRating))
        {
          result += "<li>";
          var spanClass = childPackage.Children.Any() ? "class=\"caret\"" : string.Empty;
          result += $"<span {spanClass}>" + childPackage.Name + " (" + childPackage.HotSpotRating + ")" + "</span>";
          result += Render2(childPackage, "class=\"nested\"");
          result += "</li>";
        }

        result += "</ul>";
      }

      return result;
    }
  }
}
