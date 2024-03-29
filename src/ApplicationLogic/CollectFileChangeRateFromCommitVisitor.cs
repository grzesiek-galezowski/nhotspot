using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public interface ITreeVisitor
{
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
    private readonly Maybe<RelativeDirectoryPath> _subfolder;

    public CollectFileChangeRateFromCommitVisitor(IClock clock, int minChangeCount, Maybe<RelativeDirectoryPath> subfolder)
    {
        AnalysisMetadata = new Dictionary<RelativeFilePath, FileHistoryBuilder>();
        _clock = clock;
        _minChangeCount = minChangeCount;
        _subfolder = subfolder;
    }

    private Dictionary<RelativeFilePath, FileHistoryBuilder> AnalysisMetadata { get; }

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
        RenameFile(oldPath, change.Path);
        AddChange(change);
    }

    public void OnCopied(Change change)
    {
        InitializeHistoryFor(change.Path);
        AddChange(change);
    }

    public void OnAdded(Change change)
    {
        InitializeHistoryFor(change.Path);
        AddChange(change);
    }

    public void OnRemoved(RelativeFilePath removedEntryPath)
    {
        AnalysisMetadata.Remove(removedEntryPath);
    }

    private void RenameFile(RelativeFilePath oldPath, RelativeFilePath newPath)
    {
        AnalysisMetadata[newPath] = AnalysisMetadata[oldPath];
        AnalysisMetadata.Remove(oldPath);
    }

    private void InitializeHistoryFor(RelativeFilePath relativeFilePath)
    {
        AnalysisMetadata[relativeFilePath] = new FileHistoryBuilder(_clock);
    }

    private static bool IsNotIgnoredFileType(KeyValuePair<RelativeFilePath, FileHistoryBuilder> x)
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
            ".schema", 
            ".bot", 
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

    private static IEnumerable<IFileHistory> CreateImmutableFileHistoriesFrom(IReadOnlyList<FileHistoryBuilder> trunkFiles)
    {
        //bug this method has side effects and returns a result!
        Rankings.UpdateComplexityRankingBasedOnOrderOf(OrderByComplexity(trunkFiles));
        Rankings.UpdateChangeCountRankingBasedOnOrderOf(OrderByChangesCount(trunkFiles));

        var immutableFileHistories = trunkFiles.Select(f => f.ToImmutableFileHistory());
        return immutableFileHistories;
    }

    private static IEnumerable<IFileHistoryBuilder> OrderByChangesCount(IEnumerable<IFileHistoryBuilder> trunkFiles)
    {
        return trunkFiles.ToList().OrderBy(h => h.ChangesCount());
    }

    private static IEnumerable<IFileHistoryBuilder> OrderByComplexity(IEnumerable<IFileHistoryBuilder> trunkFiles)
    {
        return trunkFiles.ToList().OrderBy(h => h.ComplexityOfCurrentVersion());
    }

    public IEnumerable<IFileHistory> Result()
    {
        var trunkFiles = AnalysisMetadata
            .Where(x => x.Value.ChangesCount() >= _minChangeCount)
            .Where(IsNotIgnoredFileType)
            .Where(IsInSpecifiedSubfolder)
            .Select(x => x.Value).ToList();
        var immutableFileHistoriesFrom = CreateImmutableFileHistoriesFrom(trunkFiles);
        return immutableFileHistoriesFrom;
    }

    private bool IsInSpecifiedSubfolder(KeyValuePair<RelativeFilePath, FileHistoryBuilder> arg)
    {
        return _subfolder.Select(sf => arg.Key.StartsWith(sf)).OrElse(true);
    }
}