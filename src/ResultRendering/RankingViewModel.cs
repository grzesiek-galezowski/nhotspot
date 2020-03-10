using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering
{
  public class RankingViewModel
  {
    public string Title { get; set; }
    public List<RankingEntryViewModel> Entries { get; } = new List<RankingEntryViewModel>();

    public static RankingViewModel NewRankingViewModel<TValue, TChangeLog, TPathType>(IEnumerable<TChangeLog> entries, Func<TChangeLog, TValue> valueFun,
      string heading) where TChangeLog : IItemWithPath<TPathType>
    {
      var rankingViewModel = new RankingViewModel {Title = heading};
      foreach (var changeLog in entries)
      {
        rankingViewModel.Entries.Add(new RankingEntryViewModel()
        {
          Name = changeLog.PathOfCurrentVersion().ToString(),
          Value = valueFun(changeLog).ToString()
        });
      }

      return rankingViewModel;
    }

    public static Task<RankingViewModel> GetRankingAsync<TValue, TChangeLog, TPathType>(
      IEnumerable<TChangeLog> entries, 
      Func<TChangeLog, TValue> valueFun, 
      string heading) where TChangeLog : IItemWithPath<TPathType>
    {
      return Task.Run(() => 
        RankingViewModel.NewRankingViewModel<TValue, TChangeLog, TPathType>(
          entries, valueFun, heading));
    }
  }
}