using System;
using static ResultRendering.Html;

namespace ResultRendering
{
  public class ResultsView
  {
    public static string Render(ViewModel viewModel)
    {
      return string.Join(Environment.NewLine,
        "<doctype html>",
        Tag("html", Attribute("lang", "en"),
          Tag("body", 
            Tag("h1", Text($"Analysis of {viewModel.RepoName}")), 
            RankingsView.RenderFrom(viewModel.Rankings), 
            PackageListView.RenderFrom(viewModel.PackageTree), 
            HotSpotsView.RenderFrom(viewModel.HotSpots)
          )
        )
    );
      /*
<doctype html>
<html lang="en">
<#= DocumentHeaderView.Render() #>
<body>
    
<h1> Analysis of <#=_viewModel.RepoName #></h1>

<#= RankingsView.RenderFrom(_viewModel.Rankings) #>
<#= PackageListView.RenderFrom(_viewModel.PackageTree) #>
<#= HotSpotsView.RenderFrom(_viewModel.HotSpots) #>

</body>
</html>
<#+private ViewModel _viewModel; #>

       */
    }
  }
}