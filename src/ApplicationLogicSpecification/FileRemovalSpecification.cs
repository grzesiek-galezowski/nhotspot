using System;
using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using FluentAssertions.Extensions;
using NHotSpot.ApplicationLogic;
using NSubstitute;
using NUnit.Framework;
using TddXt.AnyRoot.Time;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification;

public class FileRemovalSpecification
{
  [Test]
  public void ShouldNotIncludeRemovedFileInResults()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(3).Added();
        commit.File("B.cs").Complexity(5).Added();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Removed();
      });
    });

    var entries = analysisResult.EntriesByDiminishingComplexity().ToList();
    entries.Should().HaveCount(1);
    entries[0].PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
  }

  [Test]
  public void ShouldKeepOnlyNonRemovedFilesWhenMultipleFilesExist()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(1).Added();
        commit.File("B.cs").Complexity(2).Added();
        commit.File("C.cs").Complexity(3).Added();
        commit.File("D.cs").Complexity(4).Added();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Removed();
        commit.File("C.cs").Removed();
      });
    });

    var entries = analysisResult.EntriesByDiminishingComplexity().ToList();
    entries.Should().HaveCount(2);
    entries.Select(e => e.PathOfCurrentVersion()).Should().BeEquivalentTo(
    [
      RelativeFilePath.Value("B.cs"),
      RelativeFilePath.Value("D.cs")
    ]);
  }

  [Test]
  public void ShouldHandleFileRemovedAndReaddedWithNewComplexity()
  {
    var now = Any.DateTime();
    var clock = Substitute.For<IClock>();
    clock.Now().Returns(now);

    var analysisResult = new RepoAnalysisDriver { Clock = clock }.Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.Date(now - 5.Days());
        commit.File("A.cs").Complexity(1).Added();
      });
      flow.Commit(commit =>
      {
        commit.Date(now - 3.Days());
        commit.File("A.cs").Removed();
      });
      flow.Commit(commit =>
      {
        commit.Date(now - 1.Days());
        commit.File("A.cs").Complexity(10).Added();
      });
    });

    var entries = analysisResult.EntriesByDiminishingComplexity().ToList();
    entries.Should().HaveCount(1);
    entries[0].PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
    entries[0].ComplexityOfCurrentVersion().Should().Be(10);
    entries[0].ChangesCount().Should().Be(1);
  }

  [Test]
  public void ShouldReturnCorrectComplexityForLatestVersionAcrossMultipleCommits()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(1).Added();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(5).Modified();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(15).Modified();
      });
    });

    var entries = analysisResult.EntriesByDiminishingComplexity().ToList();
    entries.Should().HaveCount(1);
    entries[0].ComplexityOfCurrentVersion().Should().Be(15);
    entries[0].ChangesCount().Should().Be(3);
  }

  [Test]
  public void ShouldCalculateCorrectDatesAcrossMultipleCommits()
  {
    var now = Any.DateTime();
    var clock = Substitute.For<IClock>();
    clock.Now().Returns(now);

    var analysisResult = new RepoAnalysisDriver { Clock = clock }.Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.Date(now - 10.Days());
        commit.File("A.cs").Added();
      });
      flow.Commit(commit =>
      {
        commit.Date(now - 5.Days());
        commit.File("A.cs").Modified();
      });
      flow.Commit(commit =>
      {
        commit.Date(now - 1.Days());
        commit.File("A.cs").Modified();
      });
    });

    var entries = analysisResult.EntriesFromMostRecentlyChanged().ToList();
    entries.Should().HaveCount(1);
    entries[0].CreationDate().Should().Be(now - 10.Days());
    entries[0].LastChangeDate().Should().Be(now - 1.Days());
    entries[0].ActivityPeriod().Should().Be(9.Days());
    entries[0].TimeSinceLastChange().Should().Be(1.Days());
    entries[0].Age().Should().Be(10.Days());
  }

  [Test]
  public void ShouldNotIncludeRemovedFileEvenIfModifiedBefore()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(1).Added();
        commit.File("B.cs").Complexity(1).Added();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Complexity(5).Modified();
        commit.File("B.cs").Complexity(3).Modified();
      });
      flow.Commit(commit =>
      {
        commit.File("A.cs").Removed();
      });
    });

    var entries = analysisResult.EntriesByDiminishingComplexity().ToList();
    entries.Should().HaveCount(1);
    entries[0].PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
    entries[0].ComplexityOfCurrentVersion().Should().Be(3);
    entries[0].ChangesCount().Should().Be(2);
  }
}
