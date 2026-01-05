using System;
using AtmaFileSystem;
using Core.Maybe;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis;

public class GitRepoAnalysis(string repoPath) : IDisposable
{
  private readonly Repository _repo = new(repoPath);

  public AnalysisResult Analyze(
      Maybe<RelativeDirectoryPath> subfolder,
      string branchName,
      int minChangeCount,
      DateTime startDate)
  {
    var sourceControlRepository = GitSourceControlRepository.FromBranch(branchName, _repo, startDate);
    return new RepoAnalysis(new UtcClock(), minChangeCount, subfolder)
        .ExecuteOn(sourceControlRepository);
  }

  public void Dispose()
  {
    _repo.Dispose();
  }
}
