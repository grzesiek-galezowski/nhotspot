using System;
using ApplicationLogic;
using LibGit2Sharp;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace GitAnalysis
{
  public class SupportedBlob : IBlob
  {
    private readonly string _blobContent;

    public SupportedBlob(string blobContent)
    {
      _blobContent = blobContent;
    }

    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id)
    {
      treeVisitor.OnAdded(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, authorName, changeDate,
          changeComment, 
          id));
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id)
    {
      treeVisitor.OnModified(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent,
          authorName,
          changeDate,
          changeComment, 
          id));
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string authorName, string changeComment, string id)
    {
      treeVisitor.OnRenamed(
        RelativeFilePath(treeEntry.OldPath),
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, 
          authorName, 
          changeDate,
          changeComment, 
          id));
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id)
    {
      treeVisitor.OnCopied(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, 
          authorName, 
          changeDate,
          changeComment, 
          id));
    }
  }
}