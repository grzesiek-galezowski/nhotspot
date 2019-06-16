using System.Linq;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAttempt2
{
  public static class GitRepoAnalysis
  {
    public static AnalysisResult Analyze(string repositoryPath, string branchName)
    {
      using (var repo = new Repository(repositoryPath))
      {
        var repoPath = repo.Info.Path.Replace("\\", "/");
        var commits = repo.Branches[branchName].Commits.Reverse().ToList();
        var sourceControlRepository = new SourceControlRepository(repo, commits);

        return RepoAnalysis.ExecuteOn(sourceControlRepository, repoPath);
      }
    }
  }
}