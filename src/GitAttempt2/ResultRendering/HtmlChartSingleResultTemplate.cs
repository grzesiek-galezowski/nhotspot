using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using ApplicationLogic;

namespace ResultRendering
{
  public static class HtmlChartSingleResultTemplate
  {
      public static HotSpotViewModel FillWith(
        int elementNum, 
        FileChangeLog fileChangeLog,
        IEnumerable<Coupling> fileCouplings)
    {
      var hotSpotViewModel = new HotSpotViewModel
      {
        Complexity = fileChangeLog.ComplexityOfCurrentVersion().ToString(CultureInfo.InvariantCulture),
        ChangesCount = fileChangeLog.ChangesCount().ToString(),
        CreationDate = fileChangeLog.CreationDate().ToString("d"),
        LastChangedDate = fileChangeLog.LastChangeDate().ToString("d"),
        TimeSinceLastChanged = (int)fileChangeLog.TimeSinceLastChange().TotalDays + " days",
        ActivePeriod = (int)fileChangeLog.ActivityPeriod().TotalDays + " days",
        Age = (int)fileChangeLog.Age().TotalDays + " days",
        Path = fileChangeLog.PathOfCurrentVersion(),
        Rating = elementNum.ToString(),
        ChartValueDescription = "Complexity per change",
        Data = Data(fileChangeLog),
        Labels = Labels(fileChangeLog),
        Changes = Changes(fileChangeLog),
        ChangeCoupling = fileCouplings.Select(c => new CouplingViewModel(c.Left, c.Right, c.CouplingCount))
      };

      return hotSpotViewModel;
    }

    private static IEnumerable<ChangeViewModel> Changes(FileChangeLog analysisResult)
    {
      return analysisResult.Entries.Select(c => new ChangeViewModel(c.ChangeDate, c.Comment)).Reverse();
    }

    private static string Data(FileChangeLog fileChangeLog)
    {
        return FormatData(fileChangeLog);
    }

    private static string FormatData(FileChangeLog fileChangeLog)
    {
        var data = fileChangeLog.Entries.Select(change =>
            change.Complexity.ToString(CultureInfo.InvariantCulture));
        return TrashBinTrolololo.AsJavaScriptArrayString(data);
    }

    private static string Labels(FileChangeLog fileChangeLog)
    {
        return TrashBinTrolololo.AsJavaScriptArrayString(fileChangeLog.Entries.Select(change =>
            change.ChangeDate.ToString(Constants.CommitDateFormat, CultureInfo.InvariantCulture)));
    }
  }
}