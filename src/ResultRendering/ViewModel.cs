using System.Collections.Generic;

namespace NHotSpot.ResultRendering;

public class ViewModel
{
    public IEnumerable<HotSpotViewModel> HotSpots { get; set; } = new List<HotSpotViewModel>();
    public IEnumerable<RankingViewModel> Rankings { get; set; } = new List<RankingViewModel>();
    public string? RepoName { get; set; }
    public PackageTreeNodeViewModel? PackageTree { get; set; }
    public List<CouplingViewModel> FileCouplings { get; } = new List<CouplingViewModel>();
    public List<CouplingViewModel> PackageCouplings { get; } = new List<CouplingViewModel>();
    public HistogramViewModel? Histogram { get; set; }
}