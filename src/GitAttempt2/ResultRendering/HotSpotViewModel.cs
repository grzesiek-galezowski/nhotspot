using System.Collections.Generic;
using ApplicationLogic;

namespace ResultRendering
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
  }
}