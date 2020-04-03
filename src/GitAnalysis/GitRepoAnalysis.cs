using System;
using AtmaFileSystem;
using Functional.Maybe;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis
{
  public static class GitRepoAnalysis
  {
    public static AnalysisResult Analyze(string repositoryPath, Maybe<RelativeDirectoryPath> subfolder, string branchName, int minChangeCount,
        DateTime startDate)
    {
      using (var repo = new Repository(repositoryPath))
      {
        var sourceControlRepository = GitSourceControlRepository.FromBranch(branchName, repo, startDate);
        return new RepoAnalysis(new RealClock(), minChangeCount, subfolder)
            .ExecuteOn(sourceControlRepository);
      }
    }
  }
}