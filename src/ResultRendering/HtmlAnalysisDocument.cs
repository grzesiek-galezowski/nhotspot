using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtmaFileSystem;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering
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
      var viewModel = CreateViewModel(analysisResults);
      var contents = ResultsView.Render(viewModel, _analysisConfig);
      return contents;
    }

    private ViewModel CreateViewModel(AnalysisResult analysisResults)
    {
      var viewModel = new ViewModel();
      AddCouplingRanking(analysisResults.FileCouplingMetrics(), viewModel.FileCouplings);
      AddCouplingRanking(analysisResults.PackageCouplingMetrics(), viewModel.PackageCouplings);
      var chartDataTask = Task.Run(() => 
        HotSpotViewModel.FromAsync(
          analysisResults.EntriesByHotSpotRating(), 
          analysisResults.FileCouplingMetrics()));
      var rankingTasks = RankingTasks(analysisResults);
      var getTreeTask = Task.Run(() => PackageTreeNodeViewModel.From(analysisResults.PackageTree()));

      Task.WaitAll(rankingTasks.Concat(new Task[] {getTreeTask, chartDataTask}).ToArray());

      viewModel.Rankings = Task.WhenAll(rankingTasks).Result;
      viewModel.PackageTree = getTreeTask.Result;
      viewModel.RepoName = analysisResults.PathToRepository;
      viewModel.Histogram = HistogramViewModel.For(analysisResults.EntriesByDiminishingChangesCount());
      viewModel.HotSpots = chartDataTask.Result;
      return viewModel;
    }

    private IEnumerable<Task<RankingViewModel>> RankingTasks(AnalysisResult analysisResults)
    {
        return new []
        {
          RankingViewModel.GetRankingAsync<double, IFileHistory, RelativeFilePath>(
            analysisResults.EntriesByDiminishingComplexity(), cl => cl.ComplexityOfCurrentVersion(),
                "Most complex"),
          RankingViewModel.GetRankingAsync<double, IFileHistory, RelativeFilePath>(
            analysisResults.EntriesByDiminishingChangesCount(), cl => cl.ChangesCount(), "Most often changed"), RankingViewModel.GetRankingAsync<TimeSpan, IFileHistory, RelativeFilePath>(analysisResults.EntriesByDiminishingActivityPeriod(), cl => cl.ActivityPeriod(), "Longest active"), RankingViewModel.GetRankingAsync<string, IFileHistory, RelativeFilePath>(analysisResults.EntriesFromMostRecentlyChanged(), cl => cl.LastChangeDate().ToString("d"),
                "Most recently changed (possible breeding grounds)"),
          RankingViewModel.GetRankingAsync<string, IFileHistory, RelativeFilePath>(
            analysisResults.EntriesFromMostAncientlyChanged(), cl => cl.LastChangeDate().ToString("d"),
                "Most anciently changed (extract a library?)"),
          RankingViewModel.GetRankingAsync<double, IFlatPackageHistory, RelativeDirectoryPath>(
            analysisResults.PackagesByDiminishingHotSpotRating(), cl => cl.HotSpotRating(),
                "Package hot spots (flat)")
        };
    }

    private void AddCouplingRanking<TPath>(IEnumerable<ICoupling<TPath>> couplingMetrics, ICollection<CouplingViewModel> couplings)
    {
      foreach (var couplingViewModel in couplingMetrics.Select(CouplingViewModel.From)
          .OrderByDescending(c => c.CouplingCount))
      {
        couplings.Add(couplingViewModel);
      }
    }
  }
}