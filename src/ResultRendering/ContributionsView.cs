using System.Collections.Generic;
using System.Linq;
using NHotSpot.ResultRendering.HtmlGeneration;

namespace NHotSpot.ResultRendering;

public class ContributionsView
{
  private readonly string _title;

  public ContributionsView(string title)
  {
    _title = title;
  }

  public IEnumerable<IHtmlContent> ContributionRows(IEnumerable<ContributionViewModel> contributionViewModels)
  {
    return Html.Tag("tr",
      Html.Tag("th", Html.Text("Contributor")),
      Html.Tag("th", Html.Text("# Contributions")),
      Html.Tag("th", Html.Text("% Contributions"))
    ).Concat(
      contributionViewModels.OrderByDescending(c => c.ChangePercentage)
        .Select(contributionViewModel =>
          Html.Tr(
            Html.Td(Html.TdAttributes, Html.Text(contributionViewModel.AuthorName)),
            Html.Td(Html.TdAttributes, Html.Text(contributionViewModel.ChangeCount)),
            Html.Td(Html.TdAttributes, Html.Text(contributionViewModel.ChangePercentage.ToString("F2") + "%"))))
    );
  }

  public IHtmlContent Render(IEnumerable<ContributionViewModel> contributionViewModels)
  {
    return Html.UnrollableTable(_title, 
      ContributionRows(contributionViewModels));
  }
}