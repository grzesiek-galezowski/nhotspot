using System.Collections.Generic;
using ApplicationLogic;
using NUnit.Framework;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{
  public class Tests
  {
    [Test]
    public void METHOD()
    {
      RepoAnalysis.ExecuteOn(new MockSourceControlRepository());
    }

  }

  public class MockSourceControlRepository : ISourceControlRepository
  {
    public void CollectResults(CollectFileChangeRateFromCommitVisitor collectFileChangeRateFromCommitVisitor)
    {
      collectFileChangeRateFromCommitVisitor.
    }

    public List<string> CollectTrunkPaths()
    {
      return new List<string>();
    }

    public string Path { get; } = Any.String();
  }
}