using System.Collections.Generic;
using System.Linq;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering;

public record HotSpotViewModel(
  string Complexity,
  string ChangesCount,
  string Rating,
  string Path,
  string Age,
  string TimeSinceLastChanged,
  string ActivePeriod,
  string CreationDate,
  string LastChangedDate,
  string Labels,
  string ChartValueDescription,
  string Data,
  IEnumerable<CouplingViewModel> ChangeCoupling,
  IEnumerable<ContributionViewModel> Contributions)
{
  private static HotSpotViewModel From(IEnumerable<CouplingBetweenFiles> couplingMetrics, int i, IFileHistory log)
  {
    return HtmlChartSingleResultTemplate.FillWith(
      i + 1,
      log,
      log.Filter(couplingMetrics));
  }

  public static IEnumerable<HotSpotViewModel> From(IEnumerable<IFileHistory> entries,
    IEnumerable<CouplingBetweenFiles> couplingMetrics)
  {
    return entries.Select((history, i) => From(couplingMetrics, i, history));
  }
}