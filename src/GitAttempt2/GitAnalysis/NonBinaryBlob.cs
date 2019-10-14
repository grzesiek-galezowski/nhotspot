using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public class NonBinaryBlob : IBlob
  {
    private readonly Blob _value;

    public NonBinaryBlob(Blob value)
    {
      _value = value;
    }

    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment)
    {
      var fileText = _value.GetContentText();
      treeVisitor.OnAdded(
        ChangeFactory.CreateChange(
          treeEntryPath,
          fileText,
          changeDate,
          changeComment));
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment)
    {
      string fileText = _value.GetContentText();
      treeVisitor.OnModified(
        ChangeFactory.CreateChange(
          treeEntryPath,
          fileText,
          changeDate,
          changeComment));
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string changeComment)
    {
      string fileText = _value.GetContentText();
      treeVisitor.OnRenamed(
        treeEntry.OldPath,
        ChangeFactory.CreateChange(
          treeEntryPath,
          fileText,
          changeDate,
          changeComment));
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment)
    {
      string fileText = _value.GetContentText();
      treeVisitor.OnCopied(
        ChangeFactory.CreateChange(
          treeEntryPath,
          fileText,
          changeDate,
          changeComment));
    }
  }
}