using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLogic;
using static ResultRendering.Html;

namespace ResultRendering
{
  public static class ResultsView
  {
    public static string Render(ViewModel viewModel, AnalysisConfig analysisConfig)
    {
      var htmlContent = DocumentHeaderView.Render();
      var content = Tag("html", Attribute("lang", "en"),
        Body( 
          H(1, $"Analysis of {viewModel.RepoName}"), 
          RankingsView.RenderFrom(viewModel.Rankings), 
          CouplingView.RenderFrom(viewModel.CouplingViewModels),
          PackageListView.RenderFrom(viewModel.PackageTree), 
          HotSpotsView.RenderFrom(viewModel.HotSpots, analysisConfig)
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

  public class CouplingView
  {
    public static IHtmlContent RenderFrom(List<CouplingViewModel> viewModels)
    {
      return Tag("div",
        Tag("details",
          Tag("summary", H(2, Attribute("style", "display: inline"), "Coupling")),
          Tag("table", viewModels.Take(100).Select(model => Tr(
            Td(Text(model.CouplingCount)),
            Td(Text(model.Left)),
            Td(Text(model.Right))
          )))));
    }
  }
}