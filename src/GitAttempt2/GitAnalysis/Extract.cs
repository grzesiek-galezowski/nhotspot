using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public interface IBlob
  {
    void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment);

    void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment);

    void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string changeComment);

    void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment);
  }

  public static class Extract
  {
    public static IBlob BlobFrom(TreeEntryChanges treeEntry, Commit currentCommit)
    {
      var blob = (Blob)currentCommit[treeEntry.Path].Target;
      if (blob.IsBinary)
      {
        return new BinaryBlob();
      }
      return new NonBinaryBlob(blob);
    }
  }
}