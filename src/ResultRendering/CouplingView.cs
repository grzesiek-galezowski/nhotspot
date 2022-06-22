using System.Collections.Generic;
using System.Linq;

namespace NHotSpot.ResultRendering;

public static class CouplingView
{
    public static IHtmlContent RenderFrom(List<CouplingViewModel> viewModels, string title)
    {
        var htmlContents = Html.Tr(
                Html.Th(Html.Text("Number of common commits")),
                Html.Th(Html.Text("% left")),
                Html.Th(Html.Text("left name")),
                Html.Th(Html.Text("% right")),
                Html.Th(Html.Text("right name")),
                Html.Th(Html.Text("% of total commits"))
            )
            .Concat(
                viewModels.Take(100).Select(model =>
                    Html.Tr(
                        Html.Td(Html.Text(model.CouplingCount)),
                        Html.Td(Html.Text(model.PercentageOfLeftCommits + "%")),
                        Html.Td(Html.Strong(Html.VerbatimText(model.LongestCommonPrefix)), Html.VerbatimText(model.LeftRest)),
                        Html.Td(Html.Text(model.PercentageOfRightCommits + "%")),
                        Html.Td(Html.Strong(Html.VerbatimText(model.LongestCommonPrefix)), Html.VerbatimText(model.RightRest)),
                        Html.Td(Html.Text(model.PercentageOfTotalCommits + "%"))
                    )));
        return Html.Tag("div",
            Html.Tag("details",
                Html.Tag("summary", Html.H(2, Html.Attribute("style", "display: inline"), title)),
                Html.Tag("table",
                    htmlContents)));
    }
}