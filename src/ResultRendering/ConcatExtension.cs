using System.Collections.Generic;
using System.Linq;

namespace NHotSpot.ResultRendering
{
  public static class ConcatExtension
  {
    public static IEnumerable<T> Concat<T>(this T element, IEnumerable<T> rest)
    {
      return Enumerable.Concat(new[] { element }, rest);
    }
  }
}