using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public static class RepoAnalysis
  {
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

    public static AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
    {
      var pathsInTrunk = sourceControlRepository.CollectTrunkPaths();
      var visitor = new CollectFileChangeRateFromCommitVisitor(pathsInTrunk);
      sourceControlRepository.CollectResults(visitor);

      var trunkFiles = visitor.Result();
      var analysisResult = CreateAnalysisResult(trunkFiles, sourceControlRepository.Path);
      return analysisResult;
    }
  }
}