using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis
{
  public static class TreeNavigation
  {
    public static void Traverse(Tree tree, Commit commit, ITreeVisitor visitor)
    {
      foreach (var treeEntry in tree)
      {
        switch (treeEntry.TargetType)
        {
          case TreeEntryTargetType.Blob:
          {
            var blob = Extract.BlobFrom(commit, treeEntry.Path);
            blob.OnAdded(visitor, treeEntry.Path, commit.Author.When, commit.Author.Name, commit.Sha);
            break;
          }
          case TreeEntryTargetType.Tree:
            Traverse(treeEntry, visitor, commit);
            break;
          case TreeEntryTargetType.GitLink:
            break;
          default:
            throw new InvalidOperationException(treeEntry.Path);
        }
      }

    }

    private static void Traverse(TreeEntry treeEntry, ITreeVisitor visitor, Commit commit)
    {
      Traverse((Tree) treeEntry.Target, commit, visitor);
    }
  }
}