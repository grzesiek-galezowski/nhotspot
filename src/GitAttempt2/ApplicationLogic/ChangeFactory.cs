using System;

namespace ApplicationLogic
{
  public static class ChangeFactory
  {
    public static Change CreateChange(string path, string fileText, DateTimeOffset changeDate, string changeComment,
      string id)
    {
      return new Change(
        path, 
        fileText, ComplexityMetrics.CalculateComplexityFor(fileText), changeDate, changeComment, id);
    }
  }
}