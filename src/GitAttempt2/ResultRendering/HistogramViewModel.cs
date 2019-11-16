using System.Collections.Generic;
using System.Linq;
using ApplicationLogic;

namespace ResultRendering
{
    public class HistogramViewModel
    {
        public HistogramViewModel(string description, string labels, string data)
        {
            Labels = labels;
            Description = description;
            Data = data;
        }

        public string Labels { get; }
        public string Description { get; }
        public string Data { get; }

        public static HistogramViewModel For(IEnumerable<IFileHistory> entriesByDiminishingChangesCount)
        {
          var labels = entriesByDiminishingChangesCount.Select(e => e.PathOfCurrentVersion().FileName().ToString());
          var values = entriesByDiminishingChangesCount.Select(e => e.ChangesCount().ToString());

          return new HistogramViewModel(
            "Change count histogram",
            TrashBinTrolololo.AsJavaScriptArrayString(labels), 
            TrashBinTrolololo.AsJavaScriptArrayString(values));
        }
    }
}