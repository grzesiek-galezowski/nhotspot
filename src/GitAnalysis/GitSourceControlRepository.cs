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
    // Start from the newest commit (index 0) - these are the files we care about
    TreeNavigation.Traverse(Commits[0].Tree, Commits[0], visitor);

    var changesPerIndex = CalculateDiffsPerCommitIndex();

    // Process backwards through history (newest to oldest)
    for (var i = 1; i < Commits.Count; ++i)
    {
      var olderCommit = Commits[i];
      var changesBetweenOlderAndNewer = changesPerIndex[i];
      AnalyzeChangesBackward(
        changesBetweenOlderAndNewer,
        visitor,
        olderCommit
      );
    }
  }

  public void CollectResults(ICollectCommittInfoVisitor committVisitor)
  {
    for (var i = 1; i < Commits.Count; ++i)
    {
      var currentCommit = Commits[i];
      committVisitor.AddMetadata(
        currentCommit.Author.Name,
        currentCommit.Author.When);
    }
  }

  private ConcurrentDictionary<int, TreeChanges> CalculateDiffsPerCommitIndex()
  {
    var changesPerIndex = new ConcurrentDictionary<int, TreeChanges>();
    // Compare older commit (i) to newer commit (i-1) to see what changed going forward in time
    Parallel.For(1, Commits.Count, i =>
    {
      var newerCommit = Commits[i - 1];
      var olderCommit = Commits[i];
      changesPerIndex[i] = Repo.Diff.Compare<TreeChanges>(olderCommit.Tree, newerCommit.Tree);
    });
    return changesPerIndex;
  }

  public string Path { get; }
  public int TotalCommits { get; }

  /// &lt;summary&gt;
  /// Analyzes changes when processing commits backward (newest to oldest).
  /// The diff is computed from older commit to newer commit, showing what changed going forward in time.
  /// We process these changes to build file history, but skip files that don't exist in our target (newest) state.
  /// &lt;/summary&gt;
  private static void AnalyzeChangesBackward(
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
          // File was added in newer commit (didn't exist in older).
          // Since we're going backward, this means the file stops existing before this point.
          // We remove it from tracking so we don't try to process it in even older commits.
          treeVisitor.OnRemoved(RelativeFilePath(treeEntryPath));
          break;
        case ChangeKind.Deleted:
          // File was deleted in newer commit (existed in older).
          // Since we started from newest commit's tree, this file was never initialized.
          // Skip it - we don't care about files not in the target state.
          break;
        case ChangeKind.Modified:
          {
            // File was modified - record this change using the NEWER commit's blob
            // (we want the state after this modification was applied)
            var blob = Extract.BlobFrom(olderCommit, treeEntry.Path);
            blob.OnModified(treeVisitor, treeEntryPath, changeDate, authorName, olderCommit.Sha);
            break;
          }
        case ChangeKind.Renamed:
          {
            // File was renamed from OldPath to Path (new path) in the newer commit.
            // Going backward: we need to track this file under its OLD name for older commits.
            // This is the reverse of forward rename: we "unrename" newPath -> oldPath
            var blob = Extract.BlobFrom(olderCommit, treeEntry.OldPath);
            blob.OnRenamedBackward(treeVisitor, treeEntry.OldPath, treeEntry.Path, changeDate, authorName, olderCommit.Sha);
            break;
          }
        case ChangeKind.Copied:
          // File was copied in newer commit. Going backward, the copy target stops existing.
          treeVisitor.OnRemoved(RelativeFilePath(treeEntryPath));
          break;
        case ChangeKind.TypeChanged:
          Console.WriteLine(" type of file " + treeEntry.OldPath + " -> " + treeEntry.Path +
                            " type changed. Ignoring...");
          break;
        case ChangeKind.Ignored:
        case ChangeKind.Untracked:
        case ChangeKind.Unreadable:
        case ChangeKind.Conflicted:
          throw new NotSupportedException("Never needed to support " + treeEntry.Status + " but maybe I should?");
        default:
          throw new ArgumentOutOfRangeException(nameof(treeEntry.Status));
      }
    }
  }

  public static GitSourceControlRepository FromBranch(string branchName, Repository repo, DateTime startDate)
  {
    // Commits are sorted newest-to-oldest (CommitSortStrategies.Time is descending)
    // We keep them in this order and process backwards through history
    var commits = repo.Commits.QueryBy(new CommitFilter
    {
      IncludeReachableFrom = branchName,
      FirstParentOnly = true,
      SortBy = CommitSortStrategies.Time
    }).TakeWhile(c => c.Author.When >= startDate).ToList();
    Console.WriteLine("Starting analysis from commit " + commits.First().Sha);
    var sourceControlRepository = new GitSourceControlRepository(repo, commits);
    return sourceControlRepository;
  }
}
