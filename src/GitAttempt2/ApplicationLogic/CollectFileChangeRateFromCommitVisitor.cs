using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace ApplicationLogic
{
  public interface ITreeVisitor
  {
    void OnBlob(Change change);
    void OnModified(Change change);
    void OnRenamed(RelativeFilePath oldPath, Change change);
    void OnCopied(Change change);
    void OnAdded(Change change);
    void OnRemoved(RelativeFilePath removedEntryPath);
  }

  public class CollectFileChangeRateFromCommitVisitor : ITreeVisitor
  {
    private readonly IClock _clock;
    private readonly int _minChangeCount;

    public CollectFileChangeRateFromCommitVisitor(IClock clock, int minChangeCount)
    {
      AnalysisMetadata = new Dictionary<RelativeFilePath, FileHistory>();
      _clock = clock;
      _minChangeCount = minChangeCount;
    }

    private Dictionary<RelativeFilePath, FileHistory> AnalysisMetadata { get; }

    public void OnBlob(Change change)
    {
      if (!AnalysisMetadata.ContainsKey(change.Path))
      {
        AnalysisMetadata[change.Path] = new FileHistory(_clock);
      }
      AddChange(change);
    }

    public void OnModified(Change change)
    {
      AddChange(change);
    }

    private void AddChange(Change change)
    {
      AnalysisMetadata[change.Path].AddDataFrom(change);
    }

    public void OnRenamed(RelativeFilePath oldPath, Change change)
    {
      AnalysisMetadata[change.Path] = AnalysisMetadata[oldPath];
      AnalysisMetadata.Remove(oldPath);
      AddChange(change);
    }

    public void OnCopied(Change change)
    {
      AnalysisMetadata[change.Path] = new FileHistory(_clock);
      AddChange(change);
    }

    public void OnAdded(Change change)
    {
      AnalysisMetadata[change.Path] = new FileHistory(_clock);
      AddChange(change);
    }

    public void OnRemoved(RelativeFilePath removedEntryPath)
    {
      AnalysisMetadata.Remove(removedEntryPath);
    }

    public List<FileHistory> Result()
    {
      return AnalysisMetadata
        .Where(x => x.Value.ChangesCount() >= _minChangeCount)
        .Where(IsNotIgnoredFileType)
        .Select(x => x.Value).ToList();
    }

    private static bool IsNotIgnoredFileType(KeyValuePair<RelativeFilePath, FileHistory> x)
    {
      var ignoredFileTypes = new[] //bug move this to config
      {
        ".txt", 
        ".md", 
        ".zip", 
        ".jar", 
        ".markdown", 
        ".nuspec", 
        ".png", 
        ".jpg", 
        ".jpeg", 
        ".bmp", 
        ".yml", 
        ".json", 
        ".xml", 
        ".ico", 
        ".ruleset", 
        ".runsettings", 
        "AssemblyInfo.cs", 
        ".gitignore", 
        ".properties", 
        ".settings", 
        ".gitattributes", 
        ".csproj", 
        ".fsproj", 
        ".sln", 
        ".transcript", ".bat", 
        ".dll", ".exe", ".lock",
        ".html", ".htm", ".css"
      };
      return ignoredFileTypes.All(fileType => !x.Key.ToString().EndsWith(fileType, StringComparison.InvariantCultureIgnoreCase));
    }
  }
}