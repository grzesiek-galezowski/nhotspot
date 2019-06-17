using System;

namespace ApplicationLogic
{
  public static class ChangeFactory
  {
    public static Change CreateChange(string path, string fileText, DateTimeOffset changeDate, string changeComment)
    {
      var contentText = fileText;
      return new Change(
        path, 
        contentText, 
        ComplexityMetrics.CalculateComplexityFor(contentText),
        changeDate,
        changeComment);
    }
  }
}