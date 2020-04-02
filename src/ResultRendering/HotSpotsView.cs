using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NHotSpot.ApplicationLogic;
using NullableReferenceTypesExtensions;
using static NHotSpot.ResultRendering.Html;

namespace NHotSpot.ResultRendering
{
  public static class HotSpotsView
  {
    private static readonly HtmlAttribute[] TdAttributes = Attribute("style", "border-bottom: 1pt solid gray;");

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
        var maxCouplingPerHotSpot = analysisConfig.MaxCouplingsPerHotSpot;
        return Tag("div",
        H(2, $"{hotSpot.Rating}. {hotSpot.Path}"),
        Tag("table",
          KeyValueRow("Rating", hotSpot.Rating.OrThrow()),
          KeyValueRow("Complexity", hotSpot.Complexity.OrThrow()),
          KeyValueRow("Changes", hotSpot.ChangesCount.OrThrow()),
          KeyValueRow("Created", hotSpot.Age + " ago"),
          KeyValueRow("Last Changed", hotSpot.TimeSinceLastChanged + " ago"),
          KeyValueRow("Active for", $"{hotSpot.ActivePeriod}(First commit: {hotSpot.CreationDate}, Last: {hotSpot.LastChangedDate})")
          ), 
        chartView.ChartDiv(80),
        UnrollableTable("Contributions", ContributionRows(hotSpot)),
        UnrollableTable("History", HistoryRows(hotSpot)),
        UnrollableTable($"Coupling (Top {maxCouplingPerHotSpot})",
            CouplingRows(hotSpot, maxCouplingPerHotSpot)),
        Tag("script", Text(JavaScriptCanvas(hotSpot, chartView)))
      );
    }

    private static IHtmlContent KeyValueRow(string rating, string hotSpotRating)
    {
        return Tr(Td(Text(rating)), Td(Text(hotSpotRating)));
    }

    private static IHtmlContent UnrollableTable(string summary, IEnumerable<IHtmlContent> rows)
    {
        return Tag("details", 
            Tag("summary", Text(summary)), 
            Tag("table", rows)
        );
    }


    private static string JavaScriptCanvas(HotSpotViewModel hotSpot, ChartView chartView)
    {
        return chartView.ChartScript(hotSpot.Labels.OrThrow(), hotSpot.Data.OrThrow(), hotSpot.ChartValueDescription.OrThrow());
    }

    private static IEnumerable<IHtmlContent> ContributionRows(HotSpotViewModel hotSpot)
    {
        return Tag("tr", 
          Tag("th", Text("Contributor")), 
          Tag("th", Text("# Contributions")), 
          Tag("th", Text("% Contributions"))
        ).Concat(
          hotSpot.Contributions.OrderByDescending(c => c.ChangePercentage)
            .Select(contributionViewModel =>
                Tr(
                  Td(TdAttributes, Text(contributionViewModel.AuthorName)),
                  Td(TdAttributes, Text(contributionViewModel.ChangeCount)),
                  Td(TdAttributes, Text(contributionViewModel.ChangePercentage.ToString("F2") + "%"))))
          );
    }

    private static IEnumerable<IHtmlContent> HistoryRows(HotSpotViewModel hotSpot)
    {
      return hotSpot.Changes.Select(change =>
        Tr(Td(TdAttributes, 
                Pre(change.ChangeDate.ToString(Constants.CommitDateFormat, CultureInfo.InvariantCulture))),
          Td(TdAttributes, Pre(change.Author)),
          Td(TdAttributes, Pre(change.Comment))
          ));
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
          Td(TdAttributes, Strong(VerbatimText(change.LongestCommonPrefix)), VerbatimText(change.RightRest)),
          Td(TdAttributes, Text(change.CouplingCount)),
          Td(TdAttributes, Text(change.PercentageOfLeftCommits + "%")),
          Td(TdAttributes, Text(change.PercentageOfTotalCommits + "%"))
        )
      ));
    }

  }
}
