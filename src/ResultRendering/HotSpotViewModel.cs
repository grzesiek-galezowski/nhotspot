using System.Collections.Generic;
using System.Linq;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering
{
  public class HotSpotViewModel
  {
    public string Rating { get; set; }
    public string Path { get; set; }
    public string Complexity { get; set; }
    public string ChangesCount { get; set; }
    public string Age { get; set; }
    public string TimeSinceLastChanged { get; set; }
    public string ActivePeriod { get; set; }
    public string CreationDate { get; set; }
    public string LastChangedDate { get; set; }
    public string Labels { get; set; }
    public string ChartValueDescription { get; set; }
    public string Data { get; set; }
    public IEnumerable<ChangeViewModel> Changes { get; set; }
    public IEnumerable<CouplingViewModel> ChangeCoupling { get; set; }
    public IEnumerable<ContributionViewModel> Contributions { get; set; }

    public static HotSpotViewModel From(IEnumerable<Coupling> couplingMetrics, int i, IFileHistory log)
    {
      return HtmlChartSingleResultTemplate.FillWith(
        i + 1,
        log,
        log.Filter(couplingMetrics));
    }

    public static IEnumerable<HotSpotViewModel> FromAsync(IEnumerable<IFileHistory> entries, IEnumerable<Coupling> couplingMetrics)
    {
      return entries.Select((history, i) => HotSpotViewModel.From(couplingMetrics, i, history));
    }
  }
}