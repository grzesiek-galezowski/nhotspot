using System.Collections.Generic;
using System.Linq;

namespace NHotSpot.ResultRendering.HtmlGeneration;

public static class JavaScript
{
  public static string ArrayWith(IEnumerable<string> dataEntries)
  {
    return string.Join(", ", dataEntries.Select(entry => "'" + entry + "'"));
  }
}