using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAttempt2
{
  public class SourceControlRepository : ISourceControlRepository
  {
    public SourceControlRepository(IRepository repo, IReadOnlyCollection<Commit> commits)
    {
      Repo = repo;
      Commits = commits;
    }

    private IRepository Repo { get; }
    private IReadOnlyCollection<Commit> Commits { get; }

    public void CollectResults(CollectFileChangeRateFromCommitVisitor collectFileChangeRateFromCommitVisitor)
    {
      var treeVisitor = collectFileChangeRateFromCommitVisitor;
      TreeNavigation.Traverse(Commits.First().Tree, Commits.First(), treeVisitor);
      for (var i = 1; i < Commits.Count; ++i)
      {
        var previousCommit = Commits.ElementAt(i - 1);
        var currentCommit = Commits.ElementAt(i);

        AnalyzeChanges(
          Repo.Diff.Compare<TreeChanges>(previousCommit.Tree, currentCommit.Tree), 
          treeVisitor,
          currentCommit
        );
      }
    }

    public List<string> CollectTrunkPaths()
    {
      var pathsInTrunk = new List<string>();
      foreach (var treeEntry in Commits.Last().Tree)
      {
        switch (treeEntry.TargetType)
        {
          case TreeEntryTargetType.Blob:
            pathsInTrunk.Add(treeEntry.Path);
            break;
          case TreeEntryTargetType.Tree:
            CollectPathsFrom((Tree) treeEntry.Target, pathsInTrunk);
            break;
          case TreeEntryTargetType.GitLink:
            throw new ArgumentException(treeEntry.Path);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      return pathsInTrunk;
    }

    private static void CollectPathsFrom(Tree tree, ICollection<string> pathsByOid)
    {
      foreach (var treeEntry in tree)
      {
        switch (treeEntry.TargetType)
        {
          case TreeEntryTargetType.Blob:
            pathsByOid.Add(treeEntry.Path);
            break;
          case TreeEntryTargetType.Tree:
            CollectPathsFrom((Tree) treeEntry.Target, pathsByOid);
            break;
          case TreeEntryTargetType.GitLink:
            throw new ArgumentException(treeEntry.Path);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private static void AnalyzeChanges(
      TreeChanges treeChanges,
      CollectFileChangeRateFromCommitVisitor treeVisitor, 
      Commit currentCommit)
    {
      foreach (var treeEntry in treeChanges)
      {
        var treeEntryPath = treeEntry.Path;
        var changeDate = currentCommit.Author.When;
        var changeComment = currentCommit.Message;

        switch (treeEntry.Status)
        {
          case ChangeKind.Unmodified:
            break;
          case ChangeKind.Added:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnAdded(treeEntryPath, ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            }
            break;
          }
          case ChangeKind.Deleted:
          {
            treeVisitor.OnRemoved(treeEntryPath);
            break;
          }
          case ChangeKind.Modified:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnModified(treeEntryPath, ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            }

            break;
          }
          case ChangeKind.Renamed:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnRenamed(treeEntryPath, treeEntry.OldPath, ChangeFactory.CreateChange(treeEntry.OldPath, blob.GetContentText(), changeDate, changeComment));
            } 
            break;
          }
          case ChangeKind.Copied:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnCopied(treeEntryPath, ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            }

            break;
          }

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
  }
}