using System;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class ResultsView
  {
    public static string Render(ViewModel viewModel)
    {
      return string.Join(Environment.NewLine,
        "<doctype html>",
        DocumentHeaderView.Render(),
        Tag("html", Attribute("lang", "en"),
          Tag("body", 
            H(1, $"Analysis of {viewModel.RepoName}"), 
            RankingsView.RenderFrom(viewModel.Rankings), 
            PackageListView.RenderFrom(viewModel.PackageTree), 
            HotSpotsView.RenderFrom(viewModel.HotSpots)
          )
        )
    );
    }
  }
}