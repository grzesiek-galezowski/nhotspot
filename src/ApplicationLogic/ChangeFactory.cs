using System;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.ApplicationLogic;

public static class ChangeFactory
{
    public static Change CreateChange(
        string path, 
        Lazy<string> fileText, 
        string authorName, 
        DateTimeOffset changeDate,
        string id)
    {
        return new Change(
            RelativeFilePath(path), 
            new Lazy<double>(() => ComplexityMetrics.CalculateComplexityFor(fileText.Value)), 
            changeDate,
            authorName, 
            id);
    }
}