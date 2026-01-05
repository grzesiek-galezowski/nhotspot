using System;
using NHotSpot.ApplicationLogic;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;

namespace ApplicationLogicSpecification.Automation;

public class MockSourceControlRepository(string path, Action<IRepositoryEvolution> action) : ISourceControlRepository
{
  public static MockSourceControlRepository Default(Action<IRepositoryEvolution> action)
  {
    return new MockSourceControlRepository(Root.Any.String(), action);
  }

  public void CollectResults(ITreeVisitor visitor)
  {
    var mockTreeVisitor = new RepositoryEvolution(visitor);
    action(mockTreeVisitor);
    TotalCommits = mockTreeVisitor.CommitCount();
  }

  public void CollectResults(ICollectCommittInfoVisitor committVisitor)
  {
  }

  public string Path { get; } = path;
  public int TotalCommits { get; set; } = 0;
}
