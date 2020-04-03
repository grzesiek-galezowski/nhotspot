using System.Collections.Generic;
using AtmaFileSystem;
using Functional.Maybe;

namespace NHotSpot.ApplicationLogic
{
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
      var visitor = new CollectFileChangeRateFromCommitVisitor(_clock, _minChangeCount, _subfolder);
      sourceControlRepository.CollectResults(visitor);
      var analysisResult = CreateAnalysisResult(
        visitor.Result(), 
        sourceControlRepository.Path, 
        sourceControlRepository.TotalCommits);
      return analysisResult;
    }

    private static AnalysisResult CreateAnalysisResult(IEnumerable<IFileHistory> fileHistories, string repositoryPath,
      int totalCommits)
    {
      var immutableFileHistories = fileHistories;
      var packageHistoryNode = Rankings.GatherPackageTreeMetricsByPath(immutableFileHistories);

      var flatPackageHistoriesByPath = Rankings.GatherFlatPackageHistoriesByPath(immutableFileHistories);
      return new AnalysisResult(immutableFileHistories, 
        flatPackageHistoriesByPath, 
        repositoryPath,
        packageHistoryNode, 
        ComplexityMetrics.CalculateCoupling
          <CouplingBetweenFiles, IFileHistory, RelativeFilePath>(
                immutableFileHistories, 
                totalCommits),
        ComplexityMetrics.CalculateCoupling
          <CouplingBetweenPackages, IFlatPackageHistory, RelativeDirectoryPath>(
                flatPackageHistoriesByPath.Values, 
                totalCommits)
        );
    }
  }
}