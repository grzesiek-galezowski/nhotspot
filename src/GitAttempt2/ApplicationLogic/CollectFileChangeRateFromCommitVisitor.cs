using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public interface ITreeVisitor
  {
    void OnBlob(string filePath, string fileContent, DateTimeOffset changeDate);
  }

  public class CollectFileChangeRateFromCommitVisitor : ITreeVisitor
  {
    private readonly List<string> _pathsInTrunk;

    public CollectFileChangeRateFromCommitVisitor(List<string> pathsInTrunk)
    {
      _pathsInTrunk = pathsInTrunk;
      AnalysisMetadata = new Dictionary<string, FileChangeLog>();
    }

    private Dictionary<string, FileChangeLog> AnalysisMetadata { get; }

    public void OnBlob(string filePath, string fileContent, DateTimeOffset changeDate)
    {
      if (!AnalysisMetadata.ContainsKey(filePath))
      {
        AnalysisMetadata[filePath] = new FileChangeLog();
      }
      AddChange(filePath, fileContent, changeDate);
    }

    public void OnModified(string filePath, string fileContent, DateTimeOffset changeDate)
    {
      AddChange(filePath, fileContent, changeDate);
    }

    private void AddChange(string filePath, string fileContent, DateTimeOffset changeDate)
    {
      AnalysisMetadata[filePath].AddDataFrom(
        ChangeFactory.CreateChange(filePath, fileContent, changeDate));
    }

    public void OnRenamed(string oldPath, string newPath, string fileContent, DateTimeOffset changeDate)
    {
      AnalysisMetadata[oldPath] = AnalysisMetadata[newPath];
      AddChange(newPath, fileContent, changeDate);
    }

    public void OnCopied(string filePath, string fileContent, DateTimeOffset changeDate)
    {
      AnalysisMetadata[filePath] = new FileChangeLog();
      AddChange(filePath, fileContent, changeDate);
    }

    public void OnAdded(string filePath, string fileContent, DateTimeOffset changeDate)
    {
      AnalysisMetadata[filePath] = new FileChangeLog();
      AddChange(filePath, fileContent, changeDate);
    }

    public void OnRemoved(string treeEntryPath)
    {
      AnalysisMetadata.Remove(treeEntryPath);
    }

    public List<FileChangeLog> Result()
    {
      return AnalysisMetadata.Where(am => _pathsInTrunk.Contains(am.Key)).Select(x => x.Value).ToList();
    }
  }
}