using System;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.ApplicationLogic
{
  public static class ChangeFactory
  {
    public static Change CreateChange(string path, string fileText, string authorName, DateTimeOffset changeDate,
      string changeComment,
      string id)
    {
      return new Change(
        RelativeFilePath(path), 
        fileText, 
        new Lazy<double>(() => ComplexityMetrics.CalculateComplexityFor(fileText)), 
        changeDate,
        authorName,
        changeComment, 
        id);
    }
  }
}