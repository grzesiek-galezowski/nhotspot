using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.GitAnalysis
{
  public class GitSourceControlRepository : ISourceControlRepository
  {
    public GitSourceControlRepository(IRepository repo, IEnumerable<Commit> commits)
    {
      Repo = repo;
      Commits = commits.ToList();
      Path = repo.Info.Path.Replace("\\", "/");
      TotalCommits = Commits.Count;
    }

    private IRepository Repo { get; }
    private IReadOnlyList<Commit> Commits { get; }

    public void CollectResults(ITreeVisitor visitor)
    {
      TreeNavigation.Traverse(Commits[0].Tree, Commits[0], visitor);

      var changesPerIndex = CalculateDiffsPerCommitIndex();

      for (var i = 1; i < Commits.Count; ++i)
      {
        var currentCommit = Commits[i];
        var changesBetweenCurrentAndLastCommit = changesPerIndex[i];
        AnalyzeChanges(
          changesBetweenCurrentAndLastCommit,
          visitor,
          currentCommit
        );
      }
    }

    private ConcurrentDictionary<int, TreeChanges> CalculateDiffsPerCommitIndex()
    {
      ConcurrentDictionary<int, TreeChanges> changesPerIndex = new ConcurrentDictionary<int, TreeChanges>();
      Parallel.For(1, Commits.Count, i =>
      {
        var previousCommit = Commits[i - 1];
        var currentCommit = Commits[i];
        changesPerIndex[i] = Repo.Diff.Compare<TreeChanges>(previousCommit.Tree, currentCommit.Tree);
      });
      return changesPerIndex;
    }

    public string Path { get; }
    public int TotalCommits { get; }

    private static void AnalyzeChanges( //todo make this instance method with commit as a field
      TreeChanges treeChanges,
      ITreeVisitor treeVisitor, 
      Commit currentCommit)
    {
      foreach (var treeEntry in treeChanges) //TODO can be made async?
      {
        var treeEntryPath = treeEntry.Path;
        var changeDate = currentCommit.Author.When;
        var authorName = currentCommit.Author.Name;

        switch (treeEntry.Status)
        {
          case ChangeKind.Unmodified:
            break;
          case ChangeKind.Added:
          {
            var blob = Extract.BlobFrom(currentCommit, treeEntry.Path);
            blob.OnAdded(treeVisitor, treeEntryPath, changeDate, authorName, currentCommit.Sha);

            break;
          }
          case ChangeKind.Deleted:
          {
            treeVisitor.OnRemoved(RelativeFilePath(treeEntryPath));
            break;
          }
          case ChangeKind.Modified:
          {
            var blob = Extract.BlobFrom(currentCommit, treeEntry.Path);
            blob.OnModified(treeVisitor, treeEntryPath, changeDate, authorName, currentCommit.Sha);
            break;
          }
          case ChangeKind.Renamed:
          {
            var blob = Extract.BlobFrom(currentCommit, treeEntry.Path);
            blob.OnRenamed(treeVisitor, treeEntry, treeEntryPath, changeDate, authorName, currentCommit.Sha);
            break;
          }
          case ChangeKind.Copied:
          {
            var blob = Extract.BlobFrom(currentCommit, treeEntry.Path);
            blob.OnCopied(treeVisitor, treeEntryPath, changeDate, authorName, currentCommit.Sha);
            break;
          }
          case ChangeKind.TypeChanged:
          {
            Console.WriteLine(" type of file " + treeEntry.OldPath + " -> " + treeEntry.Path + " type changed. Ignoring...");
            break;
          }
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public static GitSourceControlRepository FromBranch(string branchName, Repository repo, DateTime startDate)
    {
      var commits = repo.Commits.QueryBy(new CommitFilter
      {
        IncludeReachableFrom = branchName,
        FirstParentOnly = true,
        SortBy = CommitSortStrategies.Time
      }).Reverse().SkipWhile(c => c.Author.When < startDate).ToList();
      Console.WriteLine("Starting analysis from commit " + commits.First().Sha);
      var sourceControlRepository = new GitSourceControlRepository(repo, commits);
      return sourceControlRepository;
    }
  }
}