using System.Collections.Generic;
using System.Linq;

namespace ResultRendering
{
  public static class ConcatExtension
  {
    public static T[] Concat<T>(this T element, IEnumerable<T> rest)
    {
      return new[] {element}.Concat(rest).ToArray();
    }
  }
}