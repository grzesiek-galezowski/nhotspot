using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using ApplicationLogic;

namespace ResultRendering
{
  public class HtmlChartOutput
  {

    public void InstantiateTemplate(AnalysisResult analysisResults)
    {
      var viewModel = new ViewModel();
      var rankings = viewModel.Rankings;
      AddRanking(analysisResults.EntriesByDiminishingComplexity(), cl => cl.ComplexityOfCurrentVersion(), "Most complex", rankings);
      AddRanking(analysisResults.EntriesByDiminishingChangesCount(), cl => cl.ChangesCount(), "Most often changed", rankings);
      AddRanking(analysisResults.EntriesByDiminishingActivityPeriod(), cl => cl.ActivityPeriod(), "Longest active", rankings);
      AddRanking(analysisResults.EntriesFromMostRecentlyChanged(), cl => cl.LastChangeDate().ToString("d"), "Most recently changed (possible breeding grounds)", rankings);
      AddRanking(analysisResults.EntriesFromMostAncientlyChanged(), cl => cl.LastChangeDate().ToString("d"), "Most anciently changed (extract a library?)", rankings);

      var charts = viewModel.HotSpots;
      AddCharts(analysisResults, charts);

      viewModel.RepoName = analysisResults.Path;

      File.WriteAllText("output.html", new HtmlTemplate(viewModel).TransformText());
    }

    private void AddRanking<T>(
      IEnumerable<ChangeLog> entries, 
      Func<ChangeLog, T> valueFun, 
      string heading,
      List<RankingViewModel> result)
    {
      var rankingViewModel = new RankingViewModel();
      rankingViewModel.Title = heading;
      foreach (var changeLog in entries.Take(20))
      {
        rankingViewModel.Entries.Add(new RankingEntryViewModel()
        {
          Name = changeLog.PathOfCurrentVersion(),
          Value = valueFun(changeLog).ToString()
        });
      }

      result.Add(rankingViewModel);
    }

    private static void AddCharts(AnalysisResult analysisResults, List<HotSpotViewModel> charts)
    {
      var elementNum = 0;
      foreach (var analysisResult in analysisResults.EntriesByHotSpotRank())
      {
        elementNum++;
        var singleFileChart = HtmlChartSingleResultTemplate.InstantiateWith(elementNum, analysisResult);
        charts.Add(singleFileChart);
      }
    }

    public void Show()
    {
      Browser.Open("output.html");
    }

  }

}