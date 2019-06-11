namespace ApplicationLogic
{
  public interface IPackageChangeLog : IChangeLog
  {
    void AddMetricsFrom(IFileChangeLog fileChangeLog); //bug this is not a common method - consider when extracting
  }
}