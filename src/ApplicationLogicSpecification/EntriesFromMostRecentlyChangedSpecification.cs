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

public class EntriesFromMostRecentlyChangedSpecification
{
    [Test]
    public void ShouldContainNoDataWhenNoCommitsDetected()
    {
        var analysisResult = new RepoAnalysisDriver().Analyze(_ => { });

        analysisResult.EntriesFromMostRecentlyChanged().Should().BeEmpty();
    }

    [Test]
    public void ShouldCalculateCorrectValuesAcrossCommits()
    {
        var now = Any.DateTime();
        var clock = Substitute.For<IClock>();
        clock.Now().Returns(now);
        var analysisResult = new RepoAnalysisDriver()
        {
            Clock = clock
        }.Analyze(flow =>
        {
            flow.Commit(commit =>
            {
                commit.Date(now - 3.Days());
                commit.File("A.cs").Added();
            });
            flow.Commit(commit =>
            {
                commit.Date(now - 2.Days());
                commit.File("B.cs").Added();
            });
            flow.Commit(commit =>
            {
                commit.Date(now - 1.Days());
                commit.File("A.cs").Modified();
            });
        });

        var entries = analysisResult.EntriesFromMostRecentlyChanged();
        entries.Should().HaveCount(2);
        entries.ElementAt(0).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("A.cs"));
        entries.ElementAt(0).LastChangeDate().Should().Be(now - 1.Days());
        entries.ElementAt(1).PathOfCurrentVersion().Should().Be(RelativeFilePath.Value("B.cs"));
        entries.ElementAt(1).LastChangeDate().Should().Be(now - 2.Days());
    }
}