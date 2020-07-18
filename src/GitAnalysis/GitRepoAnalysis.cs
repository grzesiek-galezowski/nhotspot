using System;
using AtmaFileSystem;
using Functional.Maybe;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis
{
  public class GitRepoAnalysis : IDisposable
  {
    private readonly Repository _repo;

    public GitRepoAnalysis(string repoPath)
    {
      _repo = new Repository(repoPath);
    }

    public AnalysisResult Analyze(
      Maybe<RelativeDirectoryPath> subfolder, 
      string branchName, 
      int minChangeCount,
      DateTime startDate)
    {
        var sourceControlRepository = GitSourceControlRepository.FromBranch(branchName, _repo, startDate);
        return new RepoAnalysis(new RealClock(), minChangeCount, subfolder)
            .ExecuteOn(sourceControlRepository);
    }

    public void Dispose()
    {
      _repo.Dispose();
    }
  }
}