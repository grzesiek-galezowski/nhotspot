using System.Collections.Generic;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public class RepoAnalysis(IClock clock, int minChangeCount, Maybe<RelativeDirectoryPath> subfolder)
{
  public AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
  {
    var treeVisitor = new CollectFileChangeRateFromCommitVisitor(clock, minChangeCount, subfolder);
    var commitVisitor = new CollectCommitInfoVisitor();
    sourceControlRepository.CollectResults(treeVisitor);
    sourceControlRepository.CollectResults(commitVisitor);
    var analysisResult = CreateAnalysisResult(
      treeVisitor.Result(),
      commitVisitor.TotalContributions(),
      sourceControlRepository.Path,
      sourceControlRepository.TotalCommits);
    return analysisResult;
  }

  private static AnalysisResult CreateAnalysisResult(
    IEnumerable<IFileHistory> fileHistories,
    List<Contribution> totalContributions,
    string repositoryPath,
    int totalCommits)
  {
    var packageHistoryNode = Rankings.GatherPackageTreeMetricsByPath(fileHistories);

    var flatPackageHistoriesByPath = Rankings.GatherFlatPackageHistoriesByPath(fileHistories);
    return new AnalysisResult(
      fileHistories,
      totalContributions,
      flatPackageHistoriesByPath,
      repositoryPath,
      packageHistoryNode,
      ComplexityMetrics.CalculateCoupling
        <CouplingBetweenFiles, IFileHistory, RelativeFilePath>(
          fileHistories,
          totalCommits),
      ComplexityMetrics.CalculateCoupling
        <CouplingBetweenPackages, IFlatPackageHistory, RelativeDirectoryPath>(
          flatPackageHistoriesByPath.Values,
          totalCommits)
    );
  }
}
