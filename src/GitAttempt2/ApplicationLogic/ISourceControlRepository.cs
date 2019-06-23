using System.Collections.Generic;

namespace ApplicationLogic
{
  public interface ISourceControlRepository
  {
    void CollectResults(CollectFileChangeRateFromCommitVisitor collectFileChangeRateFromCommitVisitor);
    List<string> CollectTrunkPaths();
    string Path { get; }
  }
}