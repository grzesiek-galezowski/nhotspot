using System.Collections.Generic;

namespace ResultRendering
{
  public partial class HtmlTemplate
  {
    public HtmlTemplate(ViewModel viewModel)
    {
      _viewModel = viewModel;
    }
  }

  public class ViewModel
  {
    public List<HotSpotViewModel> HotSpots { get; } = new List<HotSpotViewModel>();
    public List<RankingViewModel> Rankings { get; } = new List<RankingViewModel>();
    public string RepoName { get; set; }
  }

  public class HotSpotViewModel
  {
    public string Rank { get; set; }
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


  }

  public class RankingViewModel
  {
    public string Title { get; set; }
    public List<RankingEntryViewModel> Entries { get; } = new List<RankingEntryViewModel>();
  }

  public class RankingEntryViewModel
  {
    public string Name { get; set; }
    public string Value { get; set; }
  }
}
