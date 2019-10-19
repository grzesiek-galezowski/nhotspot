using System.Collections.Generic;

namespace ResultRendering
{
  public class ViewModel
  {
    public IList<HotSpotViewModel> HotSpots { get; } = new List<HotSpotViewModel>();
    public IEnumerable<RankingViewModel> Rankings { get; set; } = new List<RankingViewModel>();
    public string RepoName { get; set; }
    public PackageTreeNodeViewModel PackageTree { get; set; }
    public List<CouplingViewModel> CouplingViewModels { get; } = new List<CouplingViewModel>();
  }
}