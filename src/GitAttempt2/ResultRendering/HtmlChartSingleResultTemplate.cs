using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using ApplicationLogic;

namespace ResultRendering
{
  public static class HtmlChartSingleResultTemplate
  {
      public static HotSpotViewModel InstantiateWith(int elementNum, FileChangeLog analysisResult)
    {
      var hotSpotViewModel = new HotSpotViewModel
      {
        Complexity = analysisResult.ComplexityOfCurrentVersion().ToString(CultureInfo.InvariantCulture),
        ChangesCount = analysisResult.ChangesCount().ToString(),
        CreationDate = analysisResult.CreationDate().ToString("d"),
        LastChangedDate = analysisResult.LastChangeDate().ToString("d"),
        TimeSinceLastChanged = (int)analysisResult.TimeSinceLastChange().TotalDays + " days",
        ActivePeriod = (int)analysisResult.ActivityPeriod().TotalDays + " days",
        Age = (int)analysisResult.Age().TotalDays + " days",
        Path = analysisResult.PathOfCurrentVersion(),
        Rating = elementNum.ToString(),
        ChartValueDescription = "Complexity per change",
        Data = Data(analysisResult),
        Labels = Labels(analysisResult),
        Changes = Changes(analysisResult)
      };

      return hotSpotViewModel;
    }

    private static IReadOnlyList<ChangeViewModel> Changes(FileChangeLog analysisResult)
    {
      return analysisResult.Entries.Select(c => new ChangeViewModel(c.ChangeDate, c.Comment)).Reverse().ToList();
    }

    private static string Data(FileChangeLog fileChangeLog)
    {
      var data = fileChangeLog.Entries.Select(change => "'" + change.Complexity.ToString(CultureInfo.InvariantCulture) + "'");
      return string.Join(", ", data);
    }

    private static string Labels(FileChangeLog fileChangeLog)
    {
      var data = fileChangeLog.Entries.Select(change => 
        "'" + change.ChangeDate.ToString(Constants.CommittDateFormat, CultureInfo.InvariantCulture) + "'");
      return string.Join(", ", data);
    }
  }
}