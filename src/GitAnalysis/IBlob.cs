using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis;

public interface IBlob
{
  void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id);

  void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id);

  void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string authorName, string id);

  /// <summary>
  /// Handles a rename operation when processing commits backward.
  /// The file was renamed from oldPath to newPath going forward in time.
  /// Going backward, we need to track the file under its old name.
  /// </summary>
  void OnRenamedBackward(ITreeVisitor treeVisitor, string oldPath, string newPath,
      DateTimeOffset changeDate, string authorName, string id);

  void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string id);
}
