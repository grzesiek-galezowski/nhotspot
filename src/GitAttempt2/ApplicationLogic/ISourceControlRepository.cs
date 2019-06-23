namespace ApplicationLogic
{
  public interface ISourceControlRepository
  {
    void CollectResults(CollectFileChangeRateFromCommitVisitor collectFileChangeRateFromCommitVisitor);
    string Path { get; }
  }
}