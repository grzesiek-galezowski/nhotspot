using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public class NonBinaryBlob : IBlob
  {
    private readonly string _blobContent;

    public NonBinaryBlob(string blobContent)
    {
      _blobContent = blobContent;
    }

    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id)
    {
      var fileText = _blobContent;
      treeVisitor.OnAdded(
        ChangeFactory.CreateChange(
          treeEntryPath,
          fileText,
          changeDate,
          changeComment, 
          id));
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id)
    {
      treeVisitor.OnModified(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent,
          changeDate,
          changeComment, id));
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string changeComment, string id)
    {
      treeVisitor.OnRenamed(
        treeEntry.OldPath,
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent,
          changeDate,
          changeComment, id));
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id)
    {
      treeVisitor.OnCopied(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent,
          changeDate,
          changeComment, id));
    }
  }
}