﻿using System.Collections.Generic;

namespace ResultRendering
{
  public class ViewModel
  {
    public IEnumerable<HotSpotViewModel> HotSpots { get; set; } = new List<HotSpotViewModel>();
    public IEnumerable<RankingViewModel> Rankings { get; set; } = new List<RankingViewModel>();
    public string RepoName { get; set; }
    public PackageTreeNodeViewModel PackageTree { get; set; }
    public List<CouplingViewModel> Couplings { get; } = new List<CouplingViewModel>();
    public HistogramViewModel Histogram { get; set; }
  }
}