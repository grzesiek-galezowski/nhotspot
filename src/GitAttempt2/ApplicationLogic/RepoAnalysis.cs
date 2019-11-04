using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
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

      var trunkFiles = visitor.Result();
      var analysisResult = CreateAnalysisResult(trunkFiles, sourceControlRepository.Path, sourceControlRepository.TotalCommits);
      return analysisResult;
    }

    private static AnalysisResult CreateAnalysisResult(
      IReadOnlyList<FileHistoryBuilder> trunkFiles, 
      string repositoryPath,
      int totalCommits)
    {
      Rankings.UpdateComplexityRankingBasedOnOrderOf(OrderByComplexity(trunkFiles));
      Rankings.UpdateChangeCountRankingBasedOnOrderOf(OrderByChangesCount(trunkFiles));

      var immutableFileHistories = trunkFiles.Select(f => (IFileHistory)f.ToImmutableFileHistory());
      //bug convert to immutable type

      //bug remove
      var packageHistoryNode = Rankings.GatherPackageTreeMetricsByPath(immutableFileHistories);

      return new AnalysisResult(immutableFileHistories, 
        Rankings.GatherFlatPackageHistoriesByPath(immutableFileHistories), 
        repositoryPath,
        packageHistoryNode, 
        ComplexityMetrics.CalculateCoupling(immutableFileHistories, totalCommits));
    }

    private static IOrderedEnumerable<IFileHistoryBuilder> OrderByChangesCount(IEnumerable<IFileHistoryBuilder> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ChangesCount());
    }

    private static IOrderedEnumerable<IFileHistoryBuilder> OrderByComplexity(IEnumerable<IFileHistoryBuilder> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ComplexityOfCurrentVersion());
    }

  }
}