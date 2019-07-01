using System;
using System.Linq;
using ApplicationLogic;
using FluentAssertions;
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
      string file1 = Any.String();
      var changeDate1 = Any.Instance<DateTimeOffset>();
      var clock = Any.Instance<IClock>();
      var analysisResult = new RepoAnalysis(clock).ExecuteOn(new MockSourceControlRepository("REPO", v => v.OnAdded(new ChangeBuilder()
      {
        Path = file1,
        ChangeDate = changeDate1
      }.Build())));

      analysisResult.Path.Should().Be("REPO");
      analysisResult.EntriesByDiminishingActivityPeriod().Should().HaveCount(1);
      analysisResult.EntriesByDiminishingActivityPeriod().First().ActivityPeriod().Should().Be(TimeSpan.Zero);
      analysisResult.EntriesByDiminishingActivityPeriod().First().CreationDate().Should().Be(changeDate1);
      analysisResult.EntriesByDiminishingActivityPeriod().First().LastChangeDate().Should().Be(changeDate1);
      analysisResult.EntriesByDiminishingActivityPeriod().First().TimeSinceLastChange().Should().Be(clock.Now() - changeDate1);
      analysisResult.EntriesByDiminishingActivityPeriod().First().ChangesCount().Should().Be(1);
      analysisResult.EntriesByDiminishingActivityPeriod().First().ComplexityOfCurrentVersion().Should().Be(0);
      analysisResult.EntriesByDiminishingActivityPeriod().First().HotSpotRating().Should().Be((1 + 2) / 2d);
      //TODO
    }
  }

  public class MockSourceControlRepository : ISourceControlRepository
  {
    public static MockSourceControlRepository Default(Action<ITreeVisitor> action)
    {
      return new MockSourceControlRepository(Any.String(), action);
    }

    private readonly Action<ITreeVisitor> _action;

    public MockSourceControlRepository(string path, Action<ITreeVisitor> action)
    {
      _action = action;
      Path = path;
    }

    public void CollectResults(ITreeVisitor visitor)
    {
      _action(visitor);
    }

    public string Path { get; }
  }
}