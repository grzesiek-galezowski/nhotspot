using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public interface IBlob
  {
    void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id);

    void OnModified(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id);

    void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry,
      string treeEntryPath, DateTimeOffset changeDate, string changeComment, string id);

    void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath,
      DateTimeOffset changeDate, string changeComment, string id);
  }
}