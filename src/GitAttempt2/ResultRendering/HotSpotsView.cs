﻿using System.Collections.Generic;
using System.Linq;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class HotSpotsView
  {
    public static IHtmlContent RenderFrom(IEnumerable<HotSpotViewModel> viewModelHotSpots)
    {
      return Tag("div", 
        Tag("h1", VerbatimText("Hot Spots")).Concat(
        viewModelHotSpots.Select(RenderHotSpot)));
    }

    private static IHtmlContent RenderHotSpot(HotSpotViewModel hotSpot)
    {
      return Tag("div",
        Tag("h2", Text($"{hotSpot.Rating}. {hotSpot.Path}")),
        Tag("table",
          Tr(Td(Text("Rating")), Td(Text(hotSpot.Rating))),
          Tr(Td(Text("Complexity")), Td(Text(hotSpot.Complexity))),
          Tr(Td(Text("Changes")), Td(Text(hotSpot.ChangesCount))),
          Tr(Td(Text("Created")), Td(Text(hotSpot.Age + " ago"))),
          Tr(Td(Text("Last Changed")), Td(Text(hotSpot.TimeSinceLastChanged + " ago"))),
          Tr(Td(Text("Active for")), Td(Text($"{hotSpot.ActivePeriod}(First commit: {hotSpot.CreationDate}, Last: {hotSpot.LastChangedDate})")))
          ),
        Tag("table",
          Tr(Td(Tag("details", 
            Tag("summary", Text("History")), 
            Tag("table", Attribute("style", "display: block"), HistoryRows(hotSpot)))))
        ),
        Tag("div", Attribute("class", "container"),
          Tag("canvas", Attributes(("id", $"myChart{hotSpot.Rating}"), ("height", "40")))
        ),
        Tag("script", Text(JavaScriptCanvas(hotSpot)))
      );

    }

    private static string JavaScriptCanvas(HotSpotViewModel hotSpot)
    {
      var script = @"
              var ctx = document.getElementById('myChart"+ hotSpot.Rating +@"').getContext('2d');
              var chart = new Chart(ctx, {
                  // The type of chart we want to create
                  type: 'line',
                  options: {
                      elements: {
                          line: {
                              tension: 0 // disables bezier curves
                          }
                      }
                  },
                  // The data for our dataset
                  data: {
                      labels: [" + hotSpot.Labels + @"], //example '1', '2', '3'
                      datasets: [{
                          label: '"+ hotSpot.ChartValueDescription +@"',
                          fill: false,
                          borderColor: 'rgb(255, 99, 132)',
                          data: ["+ hotSpot.Data +@"]
                      }]
                  },
      
              });";
      return script;
    }

    private static IHtmlContent[] HistoryRows(HotSpotViewModel hotSpot)
    {
      return hotSpot.Changes.Select(change =>
        Tr(Td(Attribute("style", "width: 15 %; border - bottom: 1pt solid gray"), Text(change.ChangeDate)),
          Td(Attribute("style", "border - bottom: 1pt solid gray"), Pre(VerbatimText(change.Comment))))).ToArray();
    }

  }
}