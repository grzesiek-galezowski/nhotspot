using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public class RepoAnalysis
  {
    private readonly IClock _clock;

    public RepoAnalysis(IClock clock)
    {
      _clock = clock;
    }
    
    public AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
    {
      var visitor = new CollectFileChangeRateFromCommitVisitor(_clock);
      sourceControlRepository.CollectResults(visitor);

      var trunkFiles = visitor.Result();
      var analysisResult = CreateAnalysisResult(trunkFiles, sourceControlRepository.Path);
      return analysisResult;
    }

    private static AnalysisResult CreateAnalysisResult(IReadOnlyList<FileChangeLog> trunkFiles, string repositoryPath)
    {
      Rankings.UpdateComplexityRankingBasedOnOrderOf(OrderByComplexity(trunkFiles));
      Rankings.UpdateChangeCountRankingBasedOnOrderOf(OrderByChangesCount(trunkFiles));

      //bug remove
      var packageChangeLogNode = Rankings.GatherPackageTreeMetricsByPath(trunkFiles);

      return new AnalysisResult(trunkFiles, 
        Rankings.GatherFlatPackageMetricsByPath(trunkFiles), 
        repositoryPath,
        packageChangeLogNode);
    }

    private static IOrderedEnumerable<IFileChangeLog> OrderByChangesCount(IEnumerable<IFileChangeLog> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ChangesCount());
    }

    private static IOrderedEnumerable<IFileChangeLog> OrderByComplexity(IEnumerable<IFileChangeLog> trunkFiles)
    {
      return trunkFiles.ToList().OrderBy(h => h.ComplexityOfCurrentVersion());
    }

  }
}