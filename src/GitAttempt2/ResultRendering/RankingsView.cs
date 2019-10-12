using System.Collections.Generic;
using System.Linq;
using static ResultRendering.Html;

namespace ResultRendering
{
  public class RankingsView
  {
    public static IHtmlContent RenderFrom(IEnumerable<RankingViewModel> viewModelRankings)
    {
      return Tag("div", viewModelRankings.Select(ranking => Tag("details",
        RenderRankingHeader(ranking),
        RenderRankingEntries(ranking)
      )).ToArray());
    }

    private static IHtmlContent RenderRankingHeader(RankingViewModel ranking)
    {
      return Tag("summary", Tag("h2", Attribute("style", "display: inline"), VerbatimText(ranking.Title)));
    }

    private static IHtmlContent RenderRankingEntries(RankingViewModel ranking)
    {
      return Tag("ol", 
        ranking.Entries.Select(e => Tag("li", VerbatimText(e.Name + " " + e.Value))).ToArray());
    }
  }
}