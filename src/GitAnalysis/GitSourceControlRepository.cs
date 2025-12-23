using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.GitAnalysis;

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
    // Start with latest commit tree (Commits[0] is now the newest)
    TreeNavigation.Traverse(Commits[0].Tree, Commits[0], visitor);

    var changesPerIndex = CalculateDiffsPerCommitIndex();

    // Process changes going backwards in time
    for (var i = 1; i < Commits.Count; ++i)
    {
      var olderCommit = Commits[i];
      var changesBetweenCommits = changesPerIndex[i];
      AnalyzeChanges(
        changesBetweenCommits,
        visitor,
        olderCommit
      );
    }
  }

  public void CollectResults(ICollectCommittInfoVisitor committVisitor)
  {
    for (var i = 1; i < Commits.Count; ++i)
    {
      var olderCommit = Commits[i];
      committVisitor.AddMetadata(
        olderCommit.Author.Name,
        olderCommit.Author.When);
    }
  }

  private ConcurrentDictionary<int, TreeChanges> CalculateDiffsPerCommitIndex()
  {
    var changesPerIndex = new ConcurrentDictionary<int, TreeChanges>();
    Parallel.For(1, Commits.Count, i =>
    {
      var newerCommit = Commits[i - 1]; // newer in time (later chronologically)
      var olderCommit = Commits[i]; // older in time (earlier chronologically)
      // Compare older to newer to get changes going backwards in time
      changesPerIndex[i] = Repo.Diff.Compare<TreeChanges>(olderCommit.Tree, newerCommit.Tree);
    });
    return changesPerIndex;
  }

  public string Path { get; }
  public int TotalCommits { get; }

  private static void AnalyzeChanges(
    TreeChanges treeChanges,
    ITreeVisitor treeVisitor,
    Commit olderCommit)
  {
    foreach (var treeEntry in treeChanges)
    {
      var treeEntryPath = treeEntry.Path;
      var changeDate = olderCommit.Author.When;
      var authorName = olderCommit.Author.Name;

      switch (treeEntry.Status)
      {
        case ChangeKind.Unmodified:
          break;
        case ChangeKind.Added:
        {
          // When going backwards, "Added" means file was added in newer commit
// So we need to treat it as "Deleted" (file doesn't exist in older commit)
          treeVisitor.OnRemoved(RelativeFilePath(treeEntryPath));
          break;
        }
        case ChangeKind.Deleted:
        {
          // When going backwards, "Deleted" means file was deleted in newer commit
          // So we need to treat it as "Added" (file exists in older commit)
          var blob = Extract.BlobFrom(olderCommit, treeEntry.OldPath);
          blob.OnAdded(treeVisitor, treeEntry.OldPath, changeDate, authorName, olderCommit.Sha);
          break;
        }
        case ChangeKind.Modified:
        {
          var blob = Extract.BlobFrom(olderCommit, treeEntry.Path);
          blob.OnModified(treeVisitor, treeEntryPath, changeDate, authorName, olderCommit.Sha);
          break;
        }
        case ChangeKind.Renamed:
        {
          // When going backwards, rename is reversed: new path -> old path
          var blob = Extract.BlobFrom(olderCommit, treeEntry.OldPath);
          blob.OnRenamed(treeVisitor, treeEntry, treeEntry.OldPath, changeDate, authorName, olderCommit.Sha);
          break;
        }
        case ChangeKind.Copied:
        {
          // When going backwards, a copy doesn't make sense - treat as removal
          treeVisitor.OnRemoved(RelativeFilePath(treeEntryPath));
          break;
        }
        case ChangeKind.TypeChanged:
        {
          Console.WriteLine(" type of file " + treeEntry.OldPath + " -> " + treeEntry.Path +
                            " type changed. Ignoring...");
          break;
        }
        case ChangeKind.Ignored:
        case ChangeKind.Untracked:
        case ChangeKind.Unreadable:
        case ChangeKind.Conflicted:
        {
          throw new NotSupportedException("Never needed to support " + treeEntry.Status + " but maybe I should?");
        }
        default:
          throw new ArgumentOutOfRangeException(nameof(treeEntry.Status));
      }
    }
  }

  public static GitSourceControlRepository FromBranch(string branchName, Repository repo, DateTime startDate)
  {
    var commits = repo.Commits.QueryBy(new CommitFilter
    {
      IncludeReachableFrom = branchName, FirstParentOnly = true, SortBy = CommitSortStrategies.Time
    }).TakeWhile(c => c.Author.When >= startDate).ToList();
    Console.WriteLine("Starting analysis from commit " + commits.First().Sha + " (latest)");
    var sourceControlRepository = new GitSourceControlRepository(repo, commits);
    return sourceControlRepository;
  }
}
