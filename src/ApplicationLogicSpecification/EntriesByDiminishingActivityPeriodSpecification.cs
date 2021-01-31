using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using TddXt.AnyRoot.Time;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{
  public class EntriesByDiminishingActivityPeriodSpecification
  {
    [Test]
    public void ShouldContainNoDataWhenNoCommitsDetected()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

      analysisResult.EntriesByDiminishingActivityPeriod().Should().BeEmpty();
    }

    [Test]
    public void ShouldContainFoundFiledWithTheSameActivityTogether()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.File("A.cs").Added();
          commit.File("B.cs").Added();
        });
      });

      var entries = analysisResult.EntriesByDiminishingActivityPeriod();
      entries.Should().HaveCount(2);
      entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
    }
    
    [Test]
    public void ShouldSortEntriesFromLongestToShortestActiveDespiteTheAgeOfEachFile()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        var firstCommitDate = Any.DateTime();
        flow.Commit(commit =>
        {
          commit.Date(firstCommitDate);
          commit.File("A.cs").Added();
          commit.File("B.cs").Added();
          commit.File("C.cs").Added();
        });
        flow.Commit(commit =>
        {
          commit.Date(firstCommitDate + 1.Days());
          commit.File("B.cs").Modified();
        });
        flow.Commit(commit =>
        {
          commit.Date(firstCommitDate + 2.Days());
          commit.File("C.cs").Modified();
          commit.File("D.cs").Added();
        });
      });

      var entries = analysisResult.EntriesByDiminishingActivityPeriod();
      entries.Should().HaveCount(4);
      entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("C.cs"));
      entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
      entries.ElementAt(2).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(3).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("D.cs"));

      entries.ElementAt(0).ActivityPeriod().Should().Be(2.Days());
      entries.ElementAt(1).ActivityPeriod().Should().Be(1.Days());
      entries.ElementAt(2).ActivityPeriod().Should().Be(0.Days());
      entries.ElementAt(3).ActivityPeriod().Should().Be(0.Days());
    }
  }
}
