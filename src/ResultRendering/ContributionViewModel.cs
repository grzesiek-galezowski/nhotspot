using System.Collections.Generic;
using System.Linq;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering;

public record ContributionViewModel(
  decimal ChangePercentage,
  int ChangeCount,
  string AuthorName)
{
  public static IEnumerable<ContributionViewModel> ContributionsFrom(IEnumerable<Contribution> contributions)
  {
    return contributions.Select(c => new ContributionViewModel(
      c.ChangePercentage,
      c.ChangeCount,
      c.AuthorName));
  }
}