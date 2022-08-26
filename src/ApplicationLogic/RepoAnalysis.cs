using System.Collections.Generic;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public class RepoAnalysis
{
  private readonly IClock _clock;
  private readonly int _minChangeCount;
  private readonly Maybe<RelativeDirectoryPath> _subfolder;

  public RepoAnalysis(IClock clock, int minChangeCount, Maybe<RelativeDirectoryPath> subfolder)
  {
    _clock = clock;
    _minChangeCount = minChangeCount;
    _subfolder = subfolder;
  }

  public AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
  {
    var treeVisitor = new CollectFileChangeRateFromCommitVisitor(_clock, _minChangeCount, _subfolder);
    var committVisitor = new CollectCommittInfoVisitor();
    sourceControlRepository.CollectResults(treeVisitor);
    sourceControlRepository.CollectResults(committVisitor);
    var analysisResult = CreateAnalysisResult(
      treeVisitor.Result(),
      committVisitor.TotalContributions(),
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
