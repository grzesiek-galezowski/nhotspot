using System.Collections.Generic;

namespace ResultRendering
{
  public class RankingViewModel
  {
    public string Title { get; set; }
    public List<RankingEntryViewModel> Entries { get; } = new List<RankingEntryViewModel>();
  }
}