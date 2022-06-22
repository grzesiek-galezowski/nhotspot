using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NHotSpot.ApplicationLogic;
using Core.NullableReferenceTypesExtensions;

namespace NHotSpot.ResultRendering;

public class RankingViewModel
{
    public string? Title { get; set; }
    public List<RankingEntryViewModel> Entries { get; } = new List<RankingEntryViewModel>();

    public static RankingViewModel NewRankingViewModel<TValue, TChangeLog, TPathType>(
        IEnumerable<TChangeLog> entries, 
        Func<TChangeLog, TValue> valueFun,
        string heading) 
        where TChangeLog : IItemWithPath<TPathType>
        where TValue : notnull
        where TPathType : notnull
    {
        var rankingViewModel = new RankingViewModel {Title = heading};
        foreach (var changeLog in entries)
        {
            rankingViewModel.Entries.Add(
                new RankingEntryViewModel(
                    Name: changeLog.PathOfCurrentVersion().ToString().OrThrow(),
                    Value: valueFun(changeLog).ToString().OrThrow()));
        }

        return rankingViewModel;
    }

    public static Task<RankingViewModel> GetRankingAsync<TValue, TChangeLog, TPathType>(
        IEnumerable<TChangeLog> entries, 
        Func<TChangeLog, TValue> valueFun, 
        string heading) 
        where TChangeLog : IItemWithPath<TPathType>
        where TPathType : notnull
        where TValue : notnull
    {
        return Task.Run(() => 
            NewRankingViewModel<TValue, TChangeLog, TPathType>(
                entries, valueFun, heading));
    }
}