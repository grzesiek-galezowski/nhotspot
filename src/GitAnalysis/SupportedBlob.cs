using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.GitAnalysis
{
  public class SupportedBlob : IBlob
  {
    private readonly Lazy<string> _blobContent;

    public SupportedBlob(Lazy<string> blobContent)
    {
      _blobContent = blobContent;
    }

    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id)
    {
      treeVisitor.OnAdded(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, authorName, changeDate, 
          id));
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id)
    {
      treeVisitor.OnModified(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent,
          authorName,
          changeDate, 
          id));
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string authorName, string id)
    {
      treeVisitor.OnRenamed(
        RelativeFilePath(treeEntry.OldPath),
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, 
          authorName, 
          changeDate, 
          id));
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id)
    {
      treeVisitor.OnCopied(
        ChangeFactory.CreateChange(
          treeEntryPath,
          _blobContent, 
          authorName, 
          changeDate, 
          id));
    }
  }
}