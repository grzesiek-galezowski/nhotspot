using System;
using LibGit2Sharp;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.GitAnalysis
{
  public interface IBlob
  {
    void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id);

    void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id);

    void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string authorName, string changeComment, string id);

    void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string authorName, string changeComment, string id);
  }
}