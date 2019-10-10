using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
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
            var blob = (Blob) treeEntry.Target;
            if (!blob.IsBinary)
            {
              string fileText = blob.GetContentText();
              visitor.OnBlob(ChangeFactory.CreateChange(treeEntry.Path, fileText, commit.Author.When, commit.Message));
            }

            break;
          }

          case TreeEntryTargetType.Tree:
            Traverse(treeEntry, visitor, commit);
            break;
          case TreeEntryTargetType.GitLink:
            throw new ArgumentException(treeEntry.Path);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

    }

    private static void Traverse(TreeEntry treeEntry, ITreeVisitor visitor, Commit commit)
    {
      Traverse((Tree) treeEntry.Target, commit, visitor);
    }
  }
}