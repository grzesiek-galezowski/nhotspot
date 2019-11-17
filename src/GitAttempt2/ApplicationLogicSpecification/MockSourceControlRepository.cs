using System;
using ApplicationLogic;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;

namespace ApplicationLogicSpecification
{
  public class MockSourceControlRepository : ISourceControlRepository
  {
    public static MockSourceControlRepository Default(Action<IMockTreeVisitor> action)
    {
      return new MockSourceControlRepository(Root.Any.String(), action);
    }

    private readonly Action<IMockTreeVisitor> _action;

    public MockSourceControlRepository(string path, Action<IMockTreeVisitor> action)
    {
      _action = action;
      Path = path;
    }

    public void CollectResults(ITreeVisitor visitor)
    {
      var mockTreeVisitor = new MockTreeVisitor(visitor);
      _action(mockTreeVisitor);
      TotalCommits = mockTreeVisitor.CommitCount();
    }

    public string Path { get; }
    public int TotalCommits { get; set; } = 0;
  }
}