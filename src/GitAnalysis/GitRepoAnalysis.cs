using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis
{
  public static class GitRepoAnalysis
  {
    public static AnalysisResult Analyze(string repositoryPath, string branchName, int minChangeCount, DateTime startDate)
    {
      using (var repo = new Repository(repositoryPath))
      {
        var sourceControlRepository = GitSourceControlRepository.FromBranch(branchName, repo, startDate);
        return new RepoAnalysis(new RealClock(), minChangeCount).ExecuteOn(sourceControlRepository);
      }
    }
  }
}