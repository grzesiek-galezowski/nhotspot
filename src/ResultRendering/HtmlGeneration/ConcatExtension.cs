using System.Collections.Generic;
using System.Linq;

namespace NHotSpot.ResultRendering.HtmlGeneration;

public static class ConcatExtension
{
  public static IEnumerable<T> Concat<T>(this T element, IEnumerable<T> rest)
  {
    return new[] { element }.Concat(rest);
  }
}