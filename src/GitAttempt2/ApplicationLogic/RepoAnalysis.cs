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
      IReadOnlyList<FileHistory> trunkFiles, 
      string repositoryPath,
      int totalCommits)
    {
      Rankings.UpdateComplexityRankingBasedOnOrderOf(OrderByComplexity(trunkFiles));
      Rankings.UpdateChangeCountRankingBasedOnOrderOf(OrderByChangesCount(trunkFiles));

      //bug remove
      var packageChangeLogNode = Rankings.GatherPackageTreeMetricsByPath(trunkFiles);

      return new AnalysisResult(trunkFiles, 
        Rankings.GatherFlatPackageMetricsByPath(trunkFiles), 
        repositoryPath,
        packageChangeLogNode, 
        ComplexityMetrics.CalculateCoupling(trunkFiles, totalCommits));
    }

    private static IOrderedEnumerable<IFileHistory> OrderByChangesCount(IEnumerable<IFileHistory> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ChangesCount());
    }

    private static IOrderedEnumerable<IFileHistory> OrderByComplexity(IEnumerable<IFileHistory> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ComplexityOfCurrentVersion());
    }

  }
}