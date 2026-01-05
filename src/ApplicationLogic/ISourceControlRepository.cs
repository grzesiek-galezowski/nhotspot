namespace NHotSpot.ApplicationLogic;

public interface ISourceControlRepository
{
  void CollectResults(ITreeVisitor visitor);
  void CollectResults(ICollectCommittInfoVisitor committVisitor);
  string Path { get; }
  int TotalCommits { get; }
}
