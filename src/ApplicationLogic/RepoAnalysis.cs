using System.Collections.Generic;

namespace NHotSpot.ApplicationLogic
{
  public class RepoAnalysis
  {
    private readonly IClock _clock;
    private readonly int _minChangeCount;

    public RepoAnalysis(IClock clock, int minChangeCount)
    {
      _clock = clock;
      _minChangeCount = minChangeCount;
    }
    
    public AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
    {
      var visitor = new CollectFileChangeRateFromCommitVisitor(_clock, _minChangeCount);
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

      return new AnalysisResult(immutableFileHistories, 
        Rankings.GatherFlatPackageHistoriesByPath(immutableFileHistories), 
        repositoryPath,
        packageHistoryNode, 
        ComplexityMetrics.CalculateCoupling(immutableFileHistories, totalCommits));
    }
  }
}