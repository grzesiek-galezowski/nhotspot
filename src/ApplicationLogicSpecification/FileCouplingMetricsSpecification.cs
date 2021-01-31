using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification
{
  public class FileCouplingMetricsSpecification
  {
    [Test]
    public void ShouldContainNoDataWhenNoCommitsDetected()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

      analysisResult.FileCouplingMetrics().Should().BeEmpty();
    }

    [Test]
    public void ShouldContainFoundFiledWithTheSameActivityTogether()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.Dir("A1\\A2\\A3\\").File("A.cs").Added();
          commit.Dir("A1\\A2\\A21\\A3").File("B.cs").Added();
        });
      });

      var entries = analysisResult.FileCouplingMetrics().ToList();
      entries.Should().HaveCount(1);
      entries.ElementAt(0).CouplingCount.Should().Be(1);
      entries.ElementAt(0).Left.Should().Be(RelativeFilePath.Value("A1\\A2\\A3\\A.cs"));
      entries.ElementAt(0).Right.Should().Be(RelativeFilePath.Value("A1\\A2\\A21\\A3\\B.cs"));
      entries.ElementAt(0).LongestCommonPathPrefix.Should().Be("A1\\A2");
      entries.ElementAt(0).PercentageOfLeftCommits.Should().Be(100);
      entries.ElementAt(0).PercentageOfRightCommits.Should().Be(100);
      entries.ElementAt(0).PercentageOfTotalCommits.Should().Be(100);
    }
    
    [Test]
    public void CalculatePercentagesAcrossCommits()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.File("A.cs").Added();
          commit.File("B.cs").Added();
        });
        flow.Commit(commit =>
        {
          commit.File("B.cs").Modified();
        });
        flow.Commit(_ => { });
      });

      var entries = analysisResult.FileCouplingMetrics().ToList();
      entries.Should().HaveCount(1);
      entries.ElementAt(0).CouplingCount.Should().Be(1);
      entries.ElementAt(0).Left.Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(0).Right.Should().Be(RelativeFilePath.Value("B.cs"));
      entries.ElementAt(0).PercentageOfLeftCommits.Should().Be(100);
      entries.ElementAt(0).PercentageOfRightCommits.Should().Be(50);
      entries.ElementAt(0).PercentageOfTotalCommits.Should().Be(33);
    }
    
    [Test]
    public void CalculatePercentagesForDifferentCouplingOfSameFile()
    {
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.File("A.cs").Added();
          commit.File("B.cs").Added();
        });
        flow.Commit(commit =>
        {
          commit.File("A.cs").Modified();
          commit.File("C.cs").Added();
        });
        flow.Commit(_ => { });
      });

      var entries = analysisResult.FileCouplingMetrics().ToList();
      entries.Should().HaveCount(2);
      entries.ElementAt(0).CouplingCount.Should().Be(1);
      entries.ElementAt(0).Left.Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(0).PercentageOfLeftCommits.Should().Be(50);
      entries.ElementAt(0).Right.Should().Be(RelativeFilePath.Value("C.cs"));
      entries.ElementAt(0).PercentageOfRightCommits.Should().Be(100);
      entries.ElementAt(0).PercentageOfTotalCommits.Should().Be(33);

      entries.ElementAt(1).CouplingCount.Should().Be(1);
      entries.ElementAt(1).Left.Should().Be(RelativeFilePath.Value("A.cs"));
      entries.ElementAt(1).PercentageOfLeftCommits.Should().Be(50);
      entries.ElementAt(1).Right.Should().Be(RelativeFilePath.Value("B.cs"));
      entries.ElementAt(1).PercentageOfRightCommits.Should().Be(100);
      entries.ElementAt(1).PercentageOfTotalCommits.Should().Be(33);
    }
  }
}
