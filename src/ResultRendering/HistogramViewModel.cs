using System.Collections.Generic;
using System.Linq;
using NHotSpot.ApplicationLogic;
using NHotSpot.ResultRendering.HtmlGeneration;

namespace NHotSpot.ResultRendering;

public class HistogramViewModel(string description, string labels, string data)
{
  public string Labels { get; } = labels;
  public string Description { get; } = description;
  public string Data { get; } = data;

  public static HistogramViewModel For(IEnumerable<IFileHistory> entriesByDiminishingChangesCount)
  {
    var labels = entriesByDiminishingChangesCount.Select(e => e.PathOfCurrentVersion().FileName().ToString());
    var values = entriesByDiminishingChangesCount.Select(e => e.ChangesCount().ToString());

    return new HistogramViewModel(
        "Change count histogram",
        JavaScript.ArrayWith(labels),
        JavaScript.ArrayWith(values));
  }
}
