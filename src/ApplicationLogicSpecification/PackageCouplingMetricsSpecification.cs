using System.IO;
using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification;

public class PackageCouplingMetricsSpecification
{
    [Test]
    public void ShouldContainNoDataWhenNoCommitsDetected()
    {
        var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

        analysisResult.PackageCouplingMetrics().Should().BeEmpty();
    }

    [Test]
    public void ShouldCorrectlyCalculateCouplingForASingleCommit()
    {
        var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
        {
            flow.Commit(commit =>
            {
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3{Path.DirectorySeparatorChar}").File("A.cs").Added();
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3").File("B.cs").Added();
            });
        });

        var entries = analysisResult.PackageCouplingMetrics().ToList();
        entries.Should().HaveCount(1);
      
        entries.ElementAt(0).CouplingCount.Should().Be(1);
        entries.ElementAt(0).Left.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(0).Right.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(0).LongestCommonPathPrefix.Should().Be($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2");
        entries.ElementAt(0).PercentageOfLeftCommits.Should().Be(100);
        entries.ElementAt(0).PercentageOfRightCommits.Should().Be(100);
        entries.ElementAt(0).PercentageOfTotalCommits.Should().Be(100);
    }
    
    [Test]
    public void ShouldCalculatePercentagesAcrossCommits()
    {
        var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
        {
            flow.Commit(commit =>
            {
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3{Path.DirectorySeparatorChar}").File("A.cs").Added();
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3").File("B.cs").Added();
            });
            flow.Commit(commit =>
            {
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3{Path.DirectorySeparatorChar}").File("A.cs").Modified();
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3").File("B.cs").Modified();
                commit.Dir("B1").File("B.cs").Added();
            });
            flow.Commit(commit =>
            {
                commit.Dir($"A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3").File("B.cs").Modified();
                commit.Dir("C1").File("C.cs").Added();
            });
            flow.Commit(_ => { });
        });

        var entries = analysisResult.PackageCouplingMetrics().ToList().OrderBy(e => e.PercentageOfLeftCommits);
        entries.Should().HaveCount(4);
        entries.ElementAt(0).CouplingCount.Should().Be(1);
        entries.ElementAt(0).Left.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(0).Right.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}C1"));
        entries.ElementAt(0).PercentageOfLeftCommits.Should().Be(33);
        entries.ElementAt(0).PercentageOfRightCommits.Should().Be(100);
        entries.ElementAt(0).PercentageOfTotalCommits.Should().Be(25);
        entries.ElementAt(1).CouplingCount.Should().Be(1);
        entries.ElementAt(1).Left.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(1).Right.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}B1"));
        entries.ElementAt(1).PercentageOfLeftCommits.Should().Be(33);
        entries.ElementAt(1).PercentageOfRightCommits.Should().Be(100);
        entries.ElementAt(1).PercentageOfTotalCommits.Should().Be(25);
        entries.ElementAt(2).CouplingCount.Should().Be(1);
        entries.ElementAt(2).Left.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(2).Right.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}B1"));
        entries.ElementAt(2).PercentageOfLeftCommits.Should().Be(50);
        entries.ElementAt(2).PercentageOfRightCommits.Should().Be(100);
        entries.ElementAt(2).PercentageOfTotalCommits.Should().Be(25);
        entries.ElementAt(3).CouplingCount.Should().Be(2);
        entries.ElementAt(3).Left.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(3).Right.Should().Be(RelativeDirectoryPath.Value($"ROOT{Path.DirectorySeparatorChar}A1{Path.DirectorySeparatorChar}A2{Path.DirectorySeparatorChar}A21{Path.DirectorySeparatorChar}A3"));
        entries.ElementAt(3).PercentageOfLeftCommits.Should().Be(100);
        entries.ElementAt(3).PercentageOfRightCommits.Should().Be(66);
        entries.ElementAt(3).PercentageOfTotalCommits.Should().Be(50);
    }
}