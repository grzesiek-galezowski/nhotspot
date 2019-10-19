using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLogic;

namespace ResultRendering
{
  public class HtmlChartOutput
  {

    public void InstantiateTemplate(AnalysisResult analysisResults)
    {
      Console.WriteLine("START" + DateTime.Now);
      var viewModel = new ViewModel();
      AddCouplingRanking(analysisResults.CouplingMetrics(), viewModel.CouplingViewModels);
      AddChartsAsync(analysisResults, viewModel.HotSpots);
      var rankingTasks = new List<Task<RankingViewModel>>
      {
        GetRankingAsync(analysisResults.EntriesByDiminishingComplexity(), cl => cl.ComplexityOfCurrentVersion(),
          "Most complex"),
        GetRankingAsync(analysisResults.EntriesByDiminishingChangesCount(), cl => cl.ChangesCount(), "Most often changed"),
        GetRankingAsync(analysisResults.EntriesByDiminishingActivityPeriod(), cl => cl.ActivityPeriod(), "Longest active"),
        GetRankingAsync(analysisResults.EntriesFromMostRecentlyChanged(), cl => cl.LastChangeDate().ToString("d"),
          "Most recently changed (possible breeding grounds)"),
        GetRankingAsync(analysisResults.EntriesFromMostAncientlyChanged(), cl => cl.LastChangeDate().ToString("d"),
          "Most anciently changed (extract a library?)"),
        GetRankingAsync(analysisResults.PackagesByDiminishingHotSpotRating(), cl => cl.HotSpotRating(),
          "Package hot spots (flat)")
      };
      var getTreeTask = GetTree(analysisResults.PackageTree(), viewModel);

      viewModel.Rankings = Task.WhenAll(rankingTasks).Result;
      viewModel.PackageTree = getTreeTask.Result;

        Console.WriteLine(DateTime.Now);
      viewModel.RepoName = analysisResults.Path;

      var contents = ResultsView.Render(viewModel);
        Console.WriteLine("END" + DateTime.Now);
      File.WriteAllText("output.html", contents);
    }

    private void AddCouplingRanking(IEnumerable<Coupling> couplingMetrics, ICollection<CouplingViewModel> couplings)
    {
      foreach (var couplingViewModel in couplingMetrics.Select(c => new CouplingViewModel(c.Left, c.Right, c.CouplingCount)))
      {
        couplings.Add(couplingViewModel);
      }
    }

    private Task<PackageTreeNodeViewModel> GetTree(PackageChangeLogNode packageTree, ViewModel viewModel)
    {
      return Task.Run(() =>
      {
        var packageNodeViewModelVisitor = new PackageNodeViewModelVisitor();
        packageTree.Accept(packageNodeViewModelVisitor);
        return packageNodeViewModelVisitor.ToPackageNodeViewModel();
      });
    }

    private Task<RankingViewModel> GetRankingAsync<TValue, TChangeLog>(
      IEnumerable<TChangeLog> entries, 
      Func<TChangeLog, TValue> valueFun, 
      string heading) where TChangeLog : IItemWithPath
    {
      return Task.Run(() =>
      {
        var rankingViewModel = new RankingViewModel { Title = heading };
        foreach (var changeLog in entries)
        {
          rankingViewModel.Entries.Add(new RankingEntryViewModel()
          {
            Name = changeLog.PathOfCurrentVersion(),
            Value = valueFun(changeLog).ToString()
          });
        }

        return rankingViewModel;
      });
    }

    private static void AddChartsAsync(AnalysisResult analysisResults, IList<HotSpotViewModel> charts)
    {
      var couplingMetrics = analysisResults.CouplingMetrics();
      var elementNum = 0;
      foreach (var fileChangeLog in analysisResults.EntriesByHotSpotRating())
      {
        elementNum++;
        var singleFileChart = HtmlChartSingleResultTemplate.FillWith(
          elementNum, 
          fileChangeLog, 
          fileChangeLog.Filter(couplingMetrics));
        charts.Add(singleFileChart);
      }
    }

    public void Show()
    {
      Browser.Open("output.html");
    }

    public void Show(AnalysisResult analysisResult)
    {
      InstantiateTemplate(analysisResult);
      Show();
    }
  }
}