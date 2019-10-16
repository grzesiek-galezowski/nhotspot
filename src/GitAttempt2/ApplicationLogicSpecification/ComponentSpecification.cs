using System;
using System.Linq;
using ApplicationLogic;
using FluentAssertions;
using NUnit.Framework;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{
  public class ComponentSpecification
  {
    [Test]
    public void ShouldCreateRepoTreeWithNesting()
    {
      var clock = Any.Instance<IClock>();

      var analysisResult = new RepoAnalysis(clock).ExecuteOn(new MockSourceControlRepository("REPO", v =>
      {
        v.OnAdded(File("src/Readme.txt"));
        v.OnAdded(File("src/Csharp/Project1/lol.cs"));
        v.OnAdded(File("src/Csharp/Project2/lol.cs"));
        v.OnAdded(File("src/Java/src/lol.cs"));
        v.OnAdded(File("src/Java/test/lol.cs"));
      }));

      var tree = analysisResult.PackageTree();

      tree.Accept(new AssertingVisitor());
    }

    private static Change File(string file1)
    {
      return new ChangeBuilder
      {
        Path = file1,
      }.Build();
    }


    [Test] 
    public void METHOD()
    {
      string file1 = Any.String();
      var changeDate1 = Any.Instance<DateTimeOffset>();
      var clock = Any.Instance<IClock>();
      var repoPath = "REPO";
      var change1 = new ChangeBuilder
      {
        Path = file1,
        ChangeDate = changeDate1
      }.Build();

      var analysisResult = new RepoAnalysis(clock).ExecuteOn(new MockSourceControlRepository(repoPath, v =>
      {
        v.OnAdded(change1);
      }));

      analysisResult.Path.Should().Be(repoPath);
      analysisResult.EntriesByDiminishingActivityPeriod().Should().HaveCount(1);
      var fileChangeLog = (FileChangeLog)analysisResult.EntriesByDiminishingActivityPeriod().First();
      fileChangeLog.ActivityPeriod().Should().Be(TimeSpan.Zero);
      fileChangeLog.CreationDate().Should().Be(changeDate1);
      fileChangeLog.LastChangeDate().Should().Be(changeDate1);
      fileChangeLog.TimeSinceLastChange().Should().Be(clock.Now() - changeDate1);
      fileChangeLog.ChangesCount().Should().Be(1);
      fileChangeLog.ComplexityOfCurrentVersion().Should().Be(0);
      fileChangeLog.HotSpotRating().Should().Be((1 + 2) / 2d);
      fileChangeLog.Entries.Should().HaveCount(1);
      fileChangeLog.Entries.Should().BeEquivalentTo(change1);
      fileChangeLog.Age().Should().Be(clock.Now() - changeDate1);
      fileChangeLog.PathOfCurrentVersion().Should().Be(file1);
      fileChangeLog.PackagePath().Should().Be(string.Empty);
    }

    //TODO test package tree
  }

  public class AssertingVisitor : INodeVisitor
  {
    public void BeginVisiting(IFlatPackageChangeLog value)
    {
      Console.WriteLine("|" + value.PathOfCurrentVersion());
    }

    public void EndVisiting()
    {
    }

    public void Visit(IFileChangeLog fileChangeLog)
    {
      Console.WriteLine(fileChangeLog.PathOfCurrentVersion());
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