using System;
using System.Diagnostics;
using System.Linq;
using ApplicationLogic;
using ApplicationLogicSpecification.Automation;
using FluentAssertions;
using GitAnalysis;
using NUnit.Framework;
using ResultRendering;
using TddXt.AnyRoot.Strings;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static TddXt.AnyRoot.Root;
using PackagePathsWithNesting = System.Collections.Generic.List<(int nesting, AtmaFileSystem.RelativeDirectoryPath path)>;

namespace ApplicationLogicSpecification
{
  public class OldSpecification
  {
    [Test]
    public void ShouldCreateRepoTreeWithNesting()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(rootDir =>
        {
          rootDir.Dir("src", srcDir =>
          {
            srcDir.File("Readme.txt").Complexity(1).Added();
            srcDir.Dir("CSharp", csharpDir =>
            {
              csharpDir.Dir("Project1").File("lol.cs").Complexity(4).Added();
              csharpDir.Dir("Project2").File("lol.cs").Complexity(5).Added();
            });
            srcDir.Dir("Java", javaDir =>
            {
              javaDir.Dir("src").File("lol.java").Complexity(6).Added();
              javaDir.Dir("test").File("lol.java").Complexity(7).Added();
            });
          });
        });
      });

      var tree = analysisResult.PackageTree();

      var testNodeVisitor = new TestNodeVisitor();
      tree.Accept(testNodeVisitor);


      var root = RelativeDirectoryPath("ROOT");
      var src = root + RelativeDirectoryPath(@"src");
      var java = src + RelativeDirectoryPath(@"Java");
      var javasrc = java + RelativeDirectoryPath(@"src");
      var javatest = java + RelativeDirectoryPath(@"test");
      var csharp = src + RelativeDirectoryPath(@"CSharp");
      var csharpProject1 = csharp + RelativeDirectoryPath(@"Project1");
      var csharpProject2 = csharp + RelativeDirectoryPath(@"Project2");
      testNodeVisitor.OrderedPackages.Select(p => (p.nesting, p.history.PathOfCurrentVersion())).Should().BeEquivalentTo(
        new PackagePathsWithNesting
        {
          (1, root),
          (2, src),
          (3, java),
          (4, javasrc),
          (4, javatest),
          (3, csharp),
          (4, csharpProject1),
          (4, csharpProject2),
        });

      testNodeVisitor.PackagesByPath[root].ComplexityOfCurrentVersion().Should().Be(0);
      testNodeVisitor.PackagesByPath[src].ComplexityOfCurrentVersion().Should().Be(0);
      testNodeVisitor.PackagesByPath[java].ComplexityOfCurrentVersion().Should().Be(0);
      testNodeVisitor.PackagesByPath[javasrc].ComplexityOfCurrentVersion().Should().Be(6);
      testNodeVisitor.PackagesByPath[javatest].ComplexityOfCurrentVersion().Should().Be(7);
      testNodeVisitor.PackagesByPath[csharp].ComplexityOfCurrentVersion().Should().Be(0);
      testNodeVisitor.PackagesByPath[csharpProject1].ComplexityOfCurrentVersion().Should().Be(4);
      testNodeVisitor.PackagesByPath[csharpProject2].ComplexityOfCurrentVersion().Should().Be(5);

      testNodeVisitor.PackagesByPath[root]          .ChangesCount().Should().Be(0);
      testNodeVisitor.PackagesByPath[src]           .ChangesCount().Should().Be(0);
      testNodeVisitor.PackagesByPath[java]          .ChangesCount().Should().Be(0);
      testNodeVisitor.PackagesByPath[javasrc]       .ChangesCount().Should().Be(1);
      testNodeVisitor.PackagesByPath[javatest]      .ChangesCount().Should().Be(1);
      testNodeVisitor.PackagesByPath[csharp]        .ChangesCount().Should().Be(0);
      testNodeVisitor.PackagesByPath[csharpProject1].ChangesCount().Should().Be(1);
      testNodeVisitor.PackagesByPath[csharpProject2].ChangesCount().Should().Be(1);

      testNodeVisitor.PackagesByPath[root]          .HotSpotRating().Should().Be(0);
      testNodeVisitor.PackagesByPath[src]           .HotSpotRating().Should().Be(0);
      testNodeVisitor.PackagesByPath[java]          .HotSpotRating().Should().Be(0);
      testNodeVisitor.PackagesByPath[javasrc]       .HotSpotRating().Should().Be(4.5);
      testNodeVisitor.PackagesByPath[javatest]      .HotSpotRating().Should().Be(6);
      testNodeVisitor.PackagesByPath[csharp]        .HotSpotRating().Should().Be(0);
      testNodeVisitor.PackagesByPath[csharpProject1].HotSpotRating().Should().Be(1.5);
      testNodeVisitor.PackagesByPath[csharpProject2].HotSpotRating().Should().Be(3);

      testNodeVisitor.PackagesByPath[root]          .Files.Count().Should().Be(0);
      testNodeVisitor.PackagesByPath[src]           .Files.Count().Should().Be(0);
      testNodeVisitor.PackagesByPath[java]          .Files.Count().Should().Be(0);
      testNodeVisitor.PackagesByPath[javasrc]       .Files.Count().Should().Be(1);
      testNodeVisitor.PackagesByPath[javatest]      .Files.Count().Should().Be(1);
      testNodeVisitor.PackagesByPath[csharp]        .Files.Count().Should().Be(0);
      testNodeVisitor.PackagesByPath[csharpProject1].Files.Count().Should().Be(1);
      testNodeVisitor.PackagesByPath[csharpProject2].Files.Count().Should().Be(1);
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
        ChangeDate = changeDate1,
      }.Build();

      var analysisResult = new RepoAnalysis(clock, 200).ExecuteOn(new MockSourceControlRepository(repoPath, v =>
      {
        v.Add(change1);
        v.CommitChanges();
      }));

      analysisResult.PathToRepository.Should().Be(repoPath);
      analysisResult.EntriesByDiminishingActivityPeriod().Should().HaveCount(1);
      var fileChangeLog = analysisResult.EntriesByDiminishingActivityPeriod().Single();
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
      fileChangeLog.PathOfCurrentVersion().Should().Be(RelativeFilePath(file1));
      fileChangeLog.LatestPackagePath().Should().Be(string.Empty);
    }


    [Test, Explicit]
    public void Lol()
    {
        var analysisConfig = new AnalysisConfig()
        {
            //Branch = "trunk",
            Branch = "master",
            MaxCouplingsPerHotSpot = 20,
            MaxHotSpotCount = 100,
            OutputFile = "output.html",
            //RepoPath = @"c:\Users\ftw637\source\repos\vp-bots\"
            //RepoPath = @"C:\Users\ftw637\Documents\GitHub\any"
            //RepoPath = @"C:\Users\ftw637\Documents\GitHub\botbuilder-dotnet\"
            RepoPath = @"C:\Users\grzes\Documents\GitHub\NSubstitute\",
        };

        Stopwatch sw = new Stopwatch();
        sw.Start();
        //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\kafka", "trunk");
        //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\NSubstitute\", "master");
        //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\nscan\", "master");
        //var analysisResult = GitRepoAnalysis.Analyze(@"c:\Users\ftw637\source\repos\vp-bots\", "master");

        var analysisResult = GitRepoAnalysis.Analyze(analysisConfig.RepoPath, analysisConfig.Branch, 200, DateTime.Parse("2018-01-01"));
        sw.Stop();
        System.Console.WriteLine(sw.ElapsedMilliseconds);
        sw.Reset();
      
        sw.Start();
        var readyDocument = new HtmlAnalysisDocument(analysisConfig)
            .RenderString(analysisResult);
        System.IO.File.WriteAllText(analysisConfig.OutputFile, readyDocument);
        Browser.Open(analysisConfig.OutputFile);
        sw.Stop();
        System.Console.WriteLine(sw.ElapsedMilliseconds);
    }
    //TODO test package tree
  }
}