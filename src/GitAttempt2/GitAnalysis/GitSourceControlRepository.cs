using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public class GitSourceControlRepository : ISourceControlRepository
  {
    public GitSourceControlRepository(IRepository repo, IReadOnlyCollection<Commit> commits)
    {
      Repo = repo;
      Commits = commits;
      Path = repo.Info.Path.Replace("\\", "/");
    }

    private IRepository Repo { get; }
    private IReadOnlyCollection<Commit> Commits { get; }

    public void CollectResults(ITreeVisitor visitor)
    {
      var treeVisitor = visitor;
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

    public string Path { get; }

    private static void AnalyzeChanges(
      TreeChanges treeChanges,
      ITreeVisitor treeVisitor, 
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
              treeVisitor.OnAdded(ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
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
              treeVisitor.OnModified(ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            }

            break;
          }
          case ChangeKind.Renamed:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnRenamed(treeEntry.OldPath, ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            } 
            break;
          }
          case ChangeKind.Copied:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              treeVisitor.OnCopied(ChangeFactory.CreateChange(treeEntryPath, blob.GetContentText(), changeDate, changeComment));
            }

            break;
          }

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public static GitSourceControlRepository FromBranch(string branchName, Repository repo)
    {
      var commits = repo.Branches[branchName].Commits.Reverse().ToList();
      var sourceControlRepository = new GitSourceControlRepository(repo, commits);
      return sourceControlRepository;
    }
  }
}