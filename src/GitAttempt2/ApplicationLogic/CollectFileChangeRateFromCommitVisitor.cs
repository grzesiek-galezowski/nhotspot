using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public interface ITreeVisitor
  {
    void OnBlob(string filePath, Change change);
  }

  public class CollectFileChangeRateFromCommitVisitor : ITreeVisitor
  {
    public CollectFileChangeRateFromCommitVisitor()
    {
      AnalysisMetadata = new Dictionary<string, FileChangeLog>();
    }

    private Dictionary<string, FileChangeLog> AnalysisMetadata { get; }

    public void OnBlob(string filePath, Change change)
    {
      if (!AnalysisMetadata.ContainsKey(filePath))
      {
        AnalysisMetadata[filePath] = new FileChangeLog();
      }
      AddChange(filePath, change);
    }

    public void OnModified(string filePath, Change change)
    {
      AddChange(filePath, change);
    }

    private void AddChange(string filePath, Change change)
    {
      AnalysisMetadata[filePath].AddDataFrom(
        change);
    }

    public void OnRenamed(string oldPath, string newPath, Change change)
    {
      AnalysisMetadata[newPath] = AnalysisMetadata[oldPath];
      AnalysisMetadata.Remove(oldPath);
      AddChange(newPath, change);
    }

    public void OnCopied(string filePath, Change change)
    {
      AnalysisMetadata[filePath] = new FileChangeLog();
      AddChange(filePath, change);
    }

    public void OnAdded(string filePath, Change change)
    {
      AnalysisMetadata[filePath] = new FileChangeLog();
      AddChange(filePath, change);
    }

    public void OnRemoved(string treeEntryPath)
    {
      AnalysisMetadata.Remove(treeEntryPath);
    }

    public List<FileChangeLog> Result()
    {
      return AnalysisMetadata.Select(x => x.Value).ToList();
    }
  }
}