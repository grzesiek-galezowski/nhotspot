using System;
using NHotSpot.ApplicationLogic;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;

namespace ApplicationLogicSpecification.Automation
{
  public class MockSourceControlRepository : ISourceControlRepository
  {
    public static MockSourceControlRepository Default(Action<IRepositoryEvolution> action)
    {
      return new MockSourceControlRepository(Root.Any.String(), action);
    }

    private readonly Action<IRepositoryEvolution> _action;

    public MockSourceControlRepository(string path, Action<IRepositoryEvolution> action)
    {
      _action = action;
      Path = path;
    }

    public void CollectResults(ITreeVisitor visitor)
    {
      var mockTreeVisitor = new RepositoryEvolution(visitor);
      _action(mockTreeVisitor);
      TotalCommits = mockTreeVisitor.CommitCount();
    }

    public string Path { get; }
    public int TotalCommits { get; set; } = 0;
  }
}