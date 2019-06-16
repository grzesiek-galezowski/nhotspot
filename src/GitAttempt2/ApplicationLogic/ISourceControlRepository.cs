using System.Collections.Generic;
using ApplicationLogic;

namespace GitAttempt2
{
  public interface ISourceControlRepository
  {
    void CollectResults(CollectFileChangeRateFromCommitVisitor collectFileChangeRateFromCommitVisitor);
    List<string> CollectTrunkPaths();
  }
}