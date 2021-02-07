using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification
{
  public class PackagesByDiminishingHotSpotRatingSpecification
  {
    [Test]
    public void ShouldContainNoDataWhenNoCommitsDetected()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

      analysisResult.PackagesByDiminishingHotSpotRating().Should().BeEmpty();
    }

    [Test]
    public void ShouldCalculateCorrectValuesForSingleCommit()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.File("A.cs").Complexity(1).Added();
          commit.File("B.cs").Complexity(2).Added();
        });
      });

      var file1 = analysisResult.EntriesByHotSpotRating().ElementAt(0);
      var file2 = analysisResult.EntriesByHotSpotRating().ElementAt(1);
      var entries = analysisResult.PackagesByDiminishingHotSpotRating();
      entries.Should().HaveCount(1);
      entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeDirectoryPath.Value("ROOT"));
      entries.ElementAt(0).HotSpotRating().Should().Be(
        file1.HotSpotRating() + file2.HotSpotRating());
    }
    
    [Test] //bug not converted yet
    public void ShouldCalculateCorrectValuesAcrossCommits()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.File("A.cs").Complexity(1).Added();
          commit.File("B.cs").Complexity(2).Added();
          commit.File("C.cs").Complexity(3).Added();
        });
        flow.Commit(commit =>
        {
          commit.File("B.cs").Complexity(5).Modified();
          commit.File("C.cs").Complexity(6).Modified();
        });
        flow.Commit(commit =>
        {
          commit.File("A.cs").Complexity(1).Modified();
        });
        flow.Commit(commit =>
        {
          commit.File("A.cs").Complexity(1).Modified();
        });
      });

      var entries = analysisResult.EntriesByHotSpotRating();
      entries.Should().HaveCount(3);
      entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(0).HotSpotRating().Should().Be(6.5);
      entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("C.cs"));
      entries.ElementAt(1).HotSpotRating().Should().Be(5.5);
      entries.ElementAt(2).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
      entries.ElementAt(2).HotSpotRating().Should().Be(3);
    }
  }
}
