using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
      var sw = new Stopwatch();
      sw.Start();
      var treeVisitor = visitor;
      TreeNavigation.Traverse(Commits.First().Tree, Commits.First(), treeVisitor);

      var changesPerIndex = CalculateDiffsPerCommittIndex();

      for (var i = 1; i < Commits.Count; ++i)
      {
        var currentCommit = Commits.ElementAt(i);

        AnalyzeChanges(
          changesPerIndex[i],
          treeVisitor,
          currentCommit
        );
      }
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private ConcurrentDictionary<int, TreeChanges> CalculateDiffsPerCommittIndex()
    {
      ConcurrentDictionary<int, TreeChanges> changesPerIndex = new ConcurrentDictionary<int, TreeChanges>();
      Parallel.For(1, Commits.Count, i =>
      {
        var previousCommit = Commits.ElementAt(i - 1);
        var currentCommit = Commits.ElementAt(i);
        changesPerIndex[i] = Repo.Diff.Compare<TreeChanges>(previousCommit.Tree, currentCommit.Tree);
      });
      return changesPerIndex;
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
              string fileText = blob.GetContentText();
              treeVisitor.OnAdded(
                ChangeFactory.CreateChange(
                  treeEntryPath,
                  fileText,
                  changeDate,
                  changeComment));
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
              string fileText = blob.GetContentText();
              treeVisitor.OnModified(
                ChangeFactory.CreateChange(
                  treeEntryPath,
                  fileText,
                  changeDate,
                  changeComment));
            }

            break;
          }
          case ChangeKind.Renamed:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              string fileText = blob.GetContentText();
              treeVisitor.OnRenamed(
                treeEntry.OldPath,
                ChangeFactory.CreateChange(
                  treeEntryPath,
                  fileText,
                  changeDate,
                  changeComment));
            }

            break;
          }
          case ChangeKind.Copied:
          {
            var blob = LibSpecificExtractions.BlobFrom(treeEntry, currentCommit);
            if (!blob.IsBinary)
            {
              string fileText = blob.GetContentText();
              treeVisitor.OnCopied(
                ChangeFactory.CreateChange(
                  treeEntryPath,
                  fileText,
                  changeDate,
                  changeComment));
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