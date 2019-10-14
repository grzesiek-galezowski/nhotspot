using System;
using ApplicationLogic;
using LibGit2Sharp;

namespace GitAnalysis
{
  public class BinaryBlob : IBlob
  {
    public void OnAdded(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment)
    {
      
    }

    public void OnModified(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment)
    {
    }

    public void OnRenamed(ITreeVisitor treeVisitor, TreeEntryChanges treeEntry, string treeEntryPath, DateTimeOffset changeDate,
      string changeComment)
    {
    }

    public void OnCopied(ITreeVisitor treeVisitor, string treeEntryPath, DateTimeOffset changeDate, string changeComment)
    {
    }
  }
}