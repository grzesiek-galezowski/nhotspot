using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationLogicSpecification.Automation;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationLogicSpecification
{
  public class DeterminingTrunkFilesTreeSpecification
  {
    [Test]
    public void METHOD()
    {
      var date = DateTime.Now;
      var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
      {
        flow.Commit(commit =>
        {
          commit.Date(date);
          commit.File("A.cs").By("Zenek").Complexity(5).Added();
        });
      });

      analysisResult.EntriesByDiminishingActivityPeriod().Should().HaveCount(1);
      analysisResult.EntriesByDiminishingChangesCount().Should().HaveCount(1);
      analysisResult.EntriesByDiminishingComplexity().Should().HaveCount(1);
      analysisResult.EntriesByHotSpotRating().Should().HaveCount(1);
      analysisResult.EntriesFromMostAncientlyChanged().Should().HaveCount(1);
      analysisResult.EntriesFromMostRecentlyChanged().Should().HaveCount(1);
      var fileHistory = analysisResult.EntriesFromMostAncientlyChanged().ElementAt(0);

      fileHistory.Entries.Should().HaveCount(1);
      fileHistory.Entries[0].AuthorName.Should().Be("Zenek");
      fileHistory.Entries[0].ChangeDate.Should().Be(date);
      fileHistory.Entries[0].Complexity.Value.Should().Be(5);
      //bug add to test data fileHistory.Entries[0].Comment.Should().Be("5");
    }
  }
}
