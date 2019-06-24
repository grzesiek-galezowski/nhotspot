using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public interface ITreeVisitor
  {
    void OnBlob(Change change);
    void OnModified(Change change);
    void OnRenamed(string oldPath, Change change);
    void OnCopied(Change change);
    void OnAdded(Change change);
    void OnRemoved(string removedEntryPath);
  }

  public class CollectFileChangeRateFromCommitVisitor : ITreeVisitor
  {
    public CollectFileChangeRateFromCommitVisitor()
    {
      AnalysisMetadata = new Dictionary<string, FileChangeLog>();
    }

    private Dictionary<string, FileChangeLog> AnalysisMetadata { get; }

    public void OnBlob(Change change)
    {
      if (!AnalysisMetadata.ContainsKey(change.Path))
      {
        AnalysisMetadata[change.Path] = new FileChangeLog();
      }
      AddChange(change);
    }

    public void OnModified(Change change)
    {
      AddChange(change);
    }

    private void AddChange(Change change)
    {
      AnalysisMetadata[change.Path].AddDataFrom(
        change);
    }

    public void OnRenamed(string oldPath, Change change)
    {
      AnalysisMetadata[change.Path] = AnalysisMetadata[oldPath];
      AnalysisMetadata.Remove(oldPath);
      AddChange(change);
    }

    public void OnCopied(Change change)
    {
      AnalysisMetadata[change.Path] = new FileChangeLog();
      AddChange(change);
    }

    public void OnAdded(Change change)
    {
      AnalysisMetadata[change.Path] = new FileChangeLog();
      AddChange(change);
    }

    public void OnRemoved(string removedEntryPath)
    {
      AnalysisMetadata.Remove(removedEntryPath);
    }

    public List<FileChangeLog> Result()
    {
      return AnalysisMetadata.Select(x => x.Value).ToList();
    }
  }
}