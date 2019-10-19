using System;
using System.Diagnostics;
using System.Linq;
using ApplicationLogic;
using FluentAssertions;
using GitAttempt2;
using NSubstitute;
using NUnit.Framework;
using ResultRendering;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{ 
  public class ComponentSpecification
  {
    [Test]
    public void ShouldCreateRepoTreeWithNesting()
    {
      var clock = Any.Instance<IClock>();
      var nodeVisitor = Substitute.For<INodeVisitor>();

      var analysisResult = new RepoAnalysis(clock).ExecuteOn(new MockSourceControlRepository("REPO", v =>
      {
        v.OnAdded(File("src/Readme.txt"));
        v.OnAdded(File("src/Csharp/Project1/lol.cs"));
        v.OnAdded(File("src/Csharp/Project2/lol.cs"));
        v.OnAdded(File("src/Java/src/lol.cs"));
        v.OnAdded(File("src/Java/test/lol.cs"));
      }));

      var tree = analysisResult.PackageTree();

      tree.Accept(nodeVisitor);

      //THEN
      Received.InOrder(() =>
      {
        nodeVisitor.BeginVisiting(Package("src"));
        nodeVisitor.BeginVisiting(Package("CSharp"));
        nodeVisitor.EndVisiting(Package("CSharp"));
        nodeVisitor.BeginVisiting(Package("Java"));
        nodeVisitor.EndVisiting(Package("Java"));
        nodeVisitor.EndVisiting(Package("src"));
      });
    }

    private static IFlatPackageChangeLog Package(string expected)
    {
      return Arg.Is<IFlatPackageChangeLog>(
        log => log.PathOfCurrentVersion() == expected
      //  ,log => log.ChangesCount().Should().Be(1) bug
      );
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

    [Test, Ignore("lol")]
    public void METHOD123()
    {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\kafka", "trunk");
      var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\NSubstitute\", "master");
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);
      sw.Reset();
      sw.Start();
      new HtmlChartOutput().Show(analysisResult);
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);

    }

    //TODO test package tree
  }

  public class AssertingVisitor : INodeVisitor
  {
    public void BeginVisiting(IFlatPackageChangeLog value)
    {
      Console.WriteLine("|" + value.PathOfCurrentVersion());
    }

    public void EndVisiting(IFlatPackageChangeLog value)
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