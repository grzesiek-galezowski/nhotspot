using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLogic;
using AtmaFileSystem;

namespace ResultRendering
{
  public class HtmlAnalysisDocument
  {
    private readonly AnalysisConfig _analysisConfig;

    public HtmlAnalysisDocument(AnalysisConfig analysisConfig)
    {
      _analysisConfig = analysisConfig;
    }

    public string RenderString(AnalysisResult analysisResults)
    {
      var viewModel = new ViewModel();
      AddCouplingRanking(analysisResults.CouplingMetrics(), viewModel.Couplings);
      AddChartDataTo(viewModel.HotSpots, analysisResults.EntriesByHotSpotRating(), analysisResults.CouplingMetrics());
      var rankingTasks = RankingTasks(analysisResults);
      var getTreeTask = GetTree(analysisResults.PackageTree());

      viewModel.Rankings = Task.WhenAll(rankingTasks).Result;
      viewModel.PackageTree = getTreeTask.Result;
      viewModel.RepoName = analysisResults.PathToRepository;
      viewModel.Histogram = CreateHistogram(analysisResults.EntriesByDiminishingChangesCount());

      var contents = ResultsView.Render(viewModel, _analysisConfig);
      return contents;
    }

    private HistogramViewModel CreateHistogram(IEnumerable<IFileHistory> entriesByDiminishingChangesCount)
    {
      var labels = entriesByDiminishingChangesCount.Select(e => e.PathOfCurrentVersion().FileName().ToString());
      var values = entriesByDiminishingChangesCount.Select(e => e.ChangesCount().ToString());

      return new HistogramViewModel(
          "Change count histogram",
          TrashBinTrolololo.AsJavaScriptArrayString(labels), 
          TrashBinTrolololo.AsJavaScriptArrayString(values));
    }

    private IEnumerable<Task<RankingViewModel>> RankingTasks(AnalysisResult analysisResults)
    {
        return new []
        {
            GetRankingAsync<double, IFileHistory, RelativeFilePath>(analysisResults.EntriesByDiminishingComplexity(), cl => cl.ComplexityOfCurrentVersion(),
                "Most complex"),
            GetRankingAsync<double, IFileHistory, RelativeFilePath>(analysisResults.EntriesByDiminishingChangesCount(), cl => cl.ChangesCount(), "Most often changed"),
            GetRankingAsync<TimeSpan, IFileHistory, RelativeFilePath>(analysisResults.EntriesByDiminishingActivityPeriod(), cl => cl.ActivityPeriod(), "Longest active"),
            GetRankingAsync<string, IFileHistory, RelativeFilePath>(analysisResults.EntriesFromMostRecentlyChanged(), cl => cl.LastChangeDate().ToString("d"),
                "Most recently changed (possible breeding grounds)"),
            GetRankingAsync<string, IFileHistory, RelativeFilePath>(analysisResults.EntriesFromMostAncientlyChanged(), cl => cl.LastChangeDate().ToString("d"),
                "Most anciently changed (extract a library?)"),
            GetRankingAsync<double, IFlatPackageHistory, RelativeDirectoryPath>(analysisResults.PackagesByDiminishingHotSpotRating(), cl => cl.HotSpotRating(),
                "Package hot spots (flat)")
        };
    }

    private void AddCouplingRanking(IEnumerable<Coupling> couplingMetrics, ICollection<CouplingViewModel> couplings)
    {
      foreach (var couplingViewModel in 
        couplingMetrics.Select(c => new CouplingViewModel(c.Left.ToString(), c.Right.ToString(), c.CouplingCount, c.PercentageOfLeftCommits, c.PercentageOfTotalCommits)))
      {
        couplings.Add(couplingViewModel);
      }
    }

    private Task<PackageTreeNodeViewModel> GetTree(PackageHistoryNode packageTree)
    {
      return Task.Run(() =>
      {
        var packageNodeViewModelVisitor = new PackageNodeViewModelVisitor();
        packageTree.Accept(packageNodeViewModelVisitor);
        return packageNodeViewModelVisitor.ToPackageNodeViewModel();
      });
    }

    private Task<RankingViewModel> GetRankingAsync<TValue, TChangeLog, TPathType>(
      IEnumerable<TChangeLog> entries, 
      Func<TChangeLog, TValue> valueFun, 
      string heading) where TChangeLog : IItemWithPath<TPathType>
    {
      return Task.Run(() =>
      {
        var rankingViewModel = new RankingViewModel { Title = heading };
        foreach (var changeLog in entries)
        {
          rankingViewModel.Entries.Add(new RankingEntryViewModel()
          {
            Name = changeLog.PathOfCurrentVersion().ToString(),
            Value = valueFun(changeLog).ToString()
          });
        }

        return rankingViewModel;
      });
    }

    private static void AddChartDataTo(
        ICollection<HotSpotViewModel> charts, 
        IEnumerable<IFileHistory> entries, 
        IEnumerable<Coupling> couplingMetrics)
    {
      foreach (var (fileHistory, index) in entries.Select((log, i) => (log, i)))
      { 
        var singleFileChart = HtmlChartSingleResultTemplate.FillWith(
          index + 1, 
          fileHistory, 
          fileHistory.Filter(couplingMetrics));
        charts.Add(singleFileChart);
      }
    }
  }
}