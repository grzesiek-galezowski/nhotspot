using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis;

public class UnsupportedBlob : IBlob
{
  public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string authorName,
      string id)
  {

  }

  public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate,
      string authorName, string id)
  {
  }

  public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry, string treeEntryPath,
      DateTimeOffset changeDate,
      string authorName, string id)
  {
  }

  public void OnRenamedBackward(ITreeVisitor treeVisitor, string oldPath, string newPath,
      DateTimeOffset changeDate, string authorName, string id)
  {
  }

  public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate,
      string authorName, string id)
  {
  }
}
