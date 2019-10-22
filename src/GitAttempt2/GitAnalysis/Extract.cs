using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public static class Extract
  {
    public static IBlob BlobFrom(TreeEntryChanges treeEntry, Commit currentCommit)
    {
      if (currentCommit[treeEntry.Path].TargetType == TreeEntryTargetType.GitLink)
      {
        return new UnsupportedBlob();
      }
      var blob = (Blob)currentCommit[treeEntry.Path].Target;
      if (blob.IsBinary)
      {
        return new SupportedBlob(string.Empty);
      }
      return new SupportedBlob(blob.GetContentText());
    }
  }

  public class BinaryBlob : IBlob
  {
      public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment,
          string id)
      {
          
      }

      public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment,
          string id)
      {
          throw new NotImplementedException();
      }

      public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry, string treeEntryPath, DateTimeOffset changeDate,
          string changeComment, string id)
      {
          throw new NotImplementedException();
      }

      public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment,
          string id)
      {
          throw new NotImplementedException();
      }
  }
}