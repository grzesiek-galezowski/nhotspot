using System.Linq;
using ApplicationLogicSpecification.Automation;
using AtmaFileSystem;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification;

public class EntriesByDiminishingChangeCountSpecification
{
  [Test]
  public void ShouldContainNoDataWhenNoCommitsDetected()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

    analysisResult.EntriesByDiminishingChangesCount().Should().BeEmpty();
  }

  [Test]
  public void ShouldCalculateCorrectValuesForSingleCommit()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
          {
            commit.File("A.cs").Added();
            commit.File("B.cs").Added();
          });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount();
    entries.Should().HaveCount(2);
    entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
    entries.ElementAt(0).ChangesCount().Should().Be(1);
    entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
    entries.ElementAt(1).ChangesCount().Should().Be(1);
  }

  [Test]
  public void ShouldSortEntriesFromMostOftenChangedToLeastOftenChangedDespiteTheAgeOfEachFile()
  {
    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      flow.Commit(commit =>
          {
            commit.File("A.cs").Added();
            commit.File("B.cs").Added();
            commit.File("C.cs").Added();
          });
      flow.Commit(commit =>
          {
            commit.File("A.cs").Modified();
            commit.File("B.cs").Modified();
          });
      flow.Commit(commit =>
          {
            commit.File("B.cs").Modified();
            commit.File("D.cs").Added();
          });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount();
    entries.Should().HaveCount(4);
    entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
    entries.ElementAt(0).ChangesCount().Should().Be(3);
    entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
    entries.ElementAt(1).ChangesCount().Should().Be(2);
    entries.ElementAt(2).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("C.cs"));
    entries.ElementAt(2).ChangesCount().Should().Be(1);
    entries.ElementAt(3).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("D.cs"));
    entries.ElementAt(3).ChangesCount().Should().Be(1);
  }
}