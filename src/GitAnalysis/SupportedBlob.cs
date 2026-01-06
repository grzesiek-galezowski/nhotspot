using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.GitAnalysis;

public class SupportedBlob(Lazy<string> blobContent) : IBlob
{
  public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
        DateTimeOffset changeDate, string authorName, string id)
  {
    treeVisitor.OnAdded(
        ChangeFactory.CreateChange(
            treeEntryPath,
            blobContent, authorName, changeDate,
            id));
  }

  public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id)
  {
    treeVisitor.OnModified(
        ChangeFactory.CreateChange(
            treeEntryPath,
            blobContent,
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
            blobContent,
            authorName,
            changeDate,
            id));
  }

  public void OnRenamedBackward(ITreeVisitor treeVisitor, string oldPath, string newPath,
      DateTimeOffset changeDate, string authorName, string id)
  {
    // Going backward: file was renamed from oldPath to newPath going forward.
    // We need to "unrename" it: track under oldPath instead of newPath.
    treeVisitor.OnRenamed(
        RelativeFilePath(newPath),  // the name we're tracking it under currently
        ChangeFactory.CreateChange(
            oldPath,                 // the name it had in older commits
            blobContent,
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
            blobContent,
            authorName,
            changeDate,
            id));
  }
}
