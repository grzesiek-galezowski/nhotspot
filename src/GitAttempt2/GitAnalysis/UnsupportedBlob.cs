using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public class UnsupportedBlob : IBlob
  {
    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment,
      string id)
    {
      
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate,
      string changeComment, string id)
    {
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry, string treeEntryPath,
      DateTimeOffset changeDate,
      string changeComment, string id)
    {
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate,
      string changeComment, string id)
    {
    }
  }
}