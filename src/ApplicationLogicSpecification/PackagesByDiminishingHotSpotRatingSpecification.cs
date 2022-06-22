using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification;

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
                commit.Dir("A").File("B.cs").Complexity(2).Added();
                commit.Dir("A").File("C.cs").Complexity(3).Added();
            });
            flow.Commit(commit =>
            {
                commit.Dir("A").File("B.cs").Complexity(5).Modified();
                commit.Dir("A").File("C.cs").Complexity(6).Modified();
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

        var entries = analysisResult.PackagesByDiminishingHotSpotRating();
        entries.Should().HaveCount(2);
        entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeDirectoryPath.Value("ROOT") + DirectoryName.Value("A"));
        entries.ElementAt(0).HotSpotRating().Should().Be(8.5);
        entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeDirectoryPath.Value("ROOT"));
        entries.ElementAt(1).HotSpotRating().Should().Be(6.5);
    }
}