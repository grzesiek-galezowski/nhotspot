using System.Collections.Generic;

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
    public IReadOnlyList<ChangeViewModel> Changes { get; set; }
    public IEnumerable<CouplingViewModel> ChangeCoupling { get; set; }
  }
}