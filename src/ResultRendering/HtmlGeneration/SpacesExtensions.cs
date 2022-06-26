using System.Linq;

namespace NHotSpot.ResultRendering.HtmlGeneration;

public static class SpacesExtensions
{
  public static string Spaces(this int count)
  {
    return new string(Enumerable.Repeat(' ', count).ToArray());
  }
}