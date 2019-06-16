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
      for (var i = 1; i < Commits.Count(); ++i)
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
        switch (treeEntry.Status)
        {
          case ChangeKind.Unmodified:
            break;
          case ChangeKind.Added:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnAdded(treeEntry.Path, blob.GetContentText(), currentCommit.Author.When);
            }
            break;
          }
          case ChangeKind.Deleted:
          {
            treeVisitor.OnRemoved(treeEntry.Path);
            break;
          }
          case ChangeKind.Modified:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnModified(treeEntry.Path, blob.GetContentText(), currentCommit.Author.When);
            }

            break;
          }
          case ChangeKind.Renamed:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnRenamed(treeEntry.Path, treeEntry.OldPath, blob.GetContentText(), currentCommit.Author.When);
            } 
            break;
          }
          case ChangeKind.Copied:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnCopied(treeEntry.Path, blob.GetContentText(), currentCommit.Author.When);
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