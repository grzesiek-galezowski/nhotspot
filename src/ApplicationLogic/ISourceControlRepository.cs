namespace NHotSpot.ApplicationLogic
{
  public interface ISourceControlRepository
  {
    void CollectResults(ITreeVisitor visitor);
    string Path { get; }
    int TotalCommits { get; }
  }
}