﻿using System.Collections.Generic;

namespace ResultRendering
{
  public class ViewModel
  {
    public List<HotSpotViewModel> HotSpots { get; } = new List<HotSpotViewModel>();
    public List<RankingViewModel> Rankings { get; } = new List<RankingViewModel>();
    public string RepoName { get; set; }
    public PackageTreeNodeViewModel PackageTreeTree { get; set; }
  }
}