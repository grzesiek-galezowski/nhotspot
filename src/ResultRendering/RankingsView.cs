using System.Collections.Generic;
using System.Linq;
using Core.NullableReferenceTypesExtensions;
using NHotSpot.ResultRendering.HtmlGeneration;
using static NHotSpot.ResultRendering.HtmlGeneration.Html;

namespace NHotSpot.ResultRendering;

public static class RankingsView
{
  public static IHtmlContent RenderFrom(IEnumerable<RankingViewModel> viewModelRankings)
  {
    return Tag("div",
        H(1, "Rankings").Concat(
            viewModelRankings.Select(ranking =>
                Tag("details",
                    RenderRankingHeader(ranking),
                    RenderRankingEntries(ranking)
                ))));
  }

  private static IHtmlContent RenderRankingHeader(RankingViewModel ranking)
  {
    return Tag("summary", H(2, Attribute("style", "display: inline"), ranking.Title.OrThrow()));
  }

  private static IHtmlContent RenderRankingEntries(RankingViewModel ranking)
  {
    return Tag("ol",
        ranking.Entries.Select(e => Tag("li", VerbatimText(e.Name + " " + e.Value))));
  }
}