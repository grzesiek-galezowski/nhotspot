using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ApplicationLogic;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class HotSpotsView
  {
    public static IHtmlContent RenderFrom(IEnumerable<HotSpotViewModel> viewModelHotSpots, AnalysisConfig analysisConfig)
    {
      return Tag("div", 
        H(1,"Hot Spots").Concat(
        viewModelHotSpots
          .Take(analysisConfig.MaxHotSpotCount).AsParallel().AsOrdered()
          .Select(model => RenderHotSpot(model, analysisConfig))));
    }

    private static IHtmlContent RenderHotSpot(HotSpotViewModel hotSpot, AnalysisConfig analysisConfig)
    {
        var chartView = new ChartView($"myChart{hotSpot.Rating}");
        return Tag("div",
        H(2, $"{hotSpot.Rating}. {hotSpot.Path}"),
        Tag("table",
          Tr(Td(Text("Rating")), Td(Text(hotSpot.Rating))),
          Tr(Td(Text("Complexity")), Td(Text(hotSpot.Complexity))),
          Tr(Td(Text("Changes")), Td(Text(hotSpot.ChangesCount))),
          Tr(Td(Text("Created")), Td(Text(hotSpot.Age + " ago"))),
          Tr(Td(Text("Last Changed")), Td(Text(hotSpot.TimeSinceLastChanged + " ago"))),
          Tr(Td(Text("Active for")), Td(Text($"{hotSpot.ActivePeriod}(First commit: {hotSpot.CreationDate}, Last: {hotSpot.LastChangedDate})")))
          ), 
        chartView.ChartDiv(40),
        Tag("details", 
            Tag("summary", Text("Contributions")), 
            Tag("table", ContributionRows(hotSpot))
        ),
        Tag("details", 
            Tag("summary", Text("History")), 
            Tag("table", HistoryRows(hotSpot))
        ),
        Tag("details", 
            Tag("summary", Text($"Coupling (Top {analysisConfig.MaxCouplingsPerHotSpot})")), 
            Tag("table", 
              CouplingRows(hotSpot, analysisConfig.MaxCouplingsPerHotSpot))
        ),
        Tag("script", Text(JavaScriptCanvas(hotSpot, chartView)))
      );
    }


    private static string JavaScriptCanvas(HotSpotViewModel hotSpot, ChartView chartView)
    {
        return chartView.ChartScript(hotSpot.Labels, hotSpot.Data, hotSpot.ChartValueDescription);
    }

    private static IEnumerable<IHtmlContent> ContributionRows(HotSpotViewModel hotSpot)
    {
        return hotSpot.Contributions.OrderByDescending(c => c.ChangePercentage)
            .Select(contributionViewModel =>
                Tr(Td(Attribute("style", "border-bottom: 1pt solid gray;"), 
                        Text(contributionViewModel.AuthorName)),
                    Td(Attribute("style", "border-bottom: 1pt solid gray;"), 
                        Text(contributionViewModel.ChangeCount)),
                    Td(Attribute("style", "border-bottom: 1pt solid gray;"), 
                        Text(contributionViewModel.ChangePercentage.ToString("F2") + "%"))));
    }

    private static IEnumerable<IHtmlContent> HistoryRows(HotSpotViewModel hotSpot)
    {
      return hotSpot.Changes.Select(change =>
        Tr(Td(Attribute("style", "border-bottom: 1pt solid gray;"), 
                Pre(change.ChangeDate.ToString(Constants.CommitDateFormat, CultureInfo.InvariantCulture))),
          Td(Attribute("style", "border-bottom: 1pt solid gray;"), 
              Pre(change.Comment))));
    }

    private static IEnumerable<IHtmlContent> CouplingRows(HotSpotViewModel hotSpot, int count)
    {
      return Tag("tr", 
          Tag("th", Text("Filename")), 
          Tag("th", Text("Change Coupling")), 
          Tag("th", Text("% Total File Changes")),
          Tag("th", Text("% Total Changes"))
          ).Concat(hotSpot.ChangeCoupling.Take(count).Select(change =>
        Tr(
          Td(Attribute("style", "border-bottom: 1pt solid gray;"),
            Text(change.Right)),
          Td(Attribute("style", "border-bottom: 1pt solid gray;"),
            Text(change.CouplingCount),
          Td(Attribute("style", "border-bottom: 1pt solid gray;"),
            Text(change.PercentageOfLeftCommits + "%"),
          Td(Attribute("style", "border-bottom: 1pt solid gray;"),
            Text(change.PercentageOfTotalCommits + "%")
            ))))));
    }

  }
}
