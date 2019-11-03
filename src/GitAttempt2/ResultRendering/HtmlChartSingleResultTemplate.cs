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
        FileHistory fileHistory,
        IEnumerable<Coupling> fileCouplings)
    {
      var hotSpotViewModel = new HotSpotViewModel
      {
        Complexity = fileHistory.ComplexityOfCurrentVersion().ToString(CultureInfo.InvariantCulture),
        ChangesCount = fileHistory.ChangesCount().ToString(),
        CreationDate = fileHistory.CreationDate().ToString("d"),
        LastChangedDate = fileHistory.LastChangeDate().ToString("d"),
        TimeSinceLastChanged = (int)fileHistory.TimeSinceLastChange().TotalDays + " days",
        ActivePeriod = (int)fileHistory.ActivityPeriod().TotalDays + " days",
        Age = (int)fileHistory.Age().TotalDays + " days",
        Path = fileHistory.PathOfCurrentVersion().ToString(),
        Rating = elementNum.ToString(),
        ChartValueDescription = "Complexity per change",
        Data = Data(fileHistory),
        Labels = Labels(fileHistory),
        Changes = Changes(fileHistory),
        ChangeCoupling = fileCouplings.Select(c => 
          new CouplingViewModel(c.Left.ToString(), c.Right.ToString(), c.CouplingCount, c.PercentageOfLeftCommits, c.PercentageOfTotalCommits))
      };

      return hotSpotViewModel;
    }

    private static IEnumerable<ChangeViewModel> Changes(FileHistory analysisResult)
    {
      return analysisResult.Entries.Select(c => new ChangeViewModel(c.ChangeDate, c.Comment)).Reverse();
    }

    private static string Data(FileHistory fileHistory)
    {
        return FormatData(fileHistory);
    }

    private static string FormatData(FileHistory fileHistory)
    {
        var data = fileHistory.Entries.Select(change =>
            change.Complexity.ToString(CultureInfo.InvariantCulture));
        return TrashBinTrolololo.AsJavaScriptArrayString(data);
    }

    private static string Labels(FileHistory fileHistory)
    {
        return TrashBinTrolololo.AsJavaScriptArrayString(fileHistory.Entries.Select(change =>
            change.ChangeDate.ToString(Constants.CommitDateFormat, CultureInfo.InvariantCulture)));
    }
  }
}