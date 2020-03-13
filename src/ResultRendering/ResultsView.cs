using System;
using NHotSpot.ApplicationLogic;
using NullableReferenceTypesExtensions;
using static NHotSpot.ResultRendering.Html;

namespace NHotSpot.ResultRendering
{
  public static class ResultsView
  {
    public static string Render(ViewModel viewModel, AnalysisConfig analysisConfig)
    {
      var htmlContent = DocumentHeaderView.Render();
      var histogramView = new ChartView("histogram");
      var content = Tag("html", Attribute("lang", "en"),
        Body( 
          H(1, $"Analysis of {viewModel.RepoName}"),
          histogramView.ChartDiv(80),
          RankingsView.RenderFrom(viewModel.Rankings), 
          CouplingView.RenderFrom(viewModel.FileCouplings, "File Coupling"),
          CouplingView.RenderFrom(viewModel.PackageCouplings, "Package Coupling"),
          PackageListView.RenderFrom(viewModel.PackageTree), 
          HotSpotsView.RenderFrom(viewModel.HotSpots, analysisConfig),
          Tag("script", Text(histogramView.ChartScript(
            viewModel.Histogram.OrThrow().Labels, 
            viewModel.Histogram.OrThrow().Data, 
            viewModel.Histogram.OrThrow().Description)))
        )
      );
      var contentString = content.ToString();
      return string.Join(Environment.NewLine,
        "<doctype html>",
        htmlContent.ToString(),
        contentString
    );
    }
  }
}