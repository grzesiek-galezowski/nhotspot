using ApplicationLogic;
using GitAnalysis;
using LibGit2Sharp;

namespace GitAttempt2
{
  public static class GitRepoAnalysis
  {
    public static AnalysisResult Analyze(string repositoryPath, string branchName)
    {
      using (var repo = new Repository(repositoryPath))
      {
        var sourceControlRepository = GitSourceControlRepository.FromBranch(branchName, repo);

        return new RepoAnalysis(new RealClock()).ExecuteOn(sourceControlRepository);
      }
    }
  }
}