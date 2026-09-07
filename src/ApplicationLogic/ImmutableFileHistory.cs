using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public interface IFileHistory : IItemHistory<RelativeFilePath>, ICouplingSource<CouplingBetweenFiles, IFileHistory>
{
  DateTimeOffset LastChangeDate();
  TimeSpan ActivityPeriod();
  IEnumerable<string> ChangeIds();
  IEnumerable<CouplingBetweenFiles> Filter(IEnumerable<CouplingBetweenFiles> couplingMetrics);
  DateTimeOffset CreationDate();
  TimeSpan TimeSinceLastChange();
  TimeSpan Age();
  IReadOnlyList<Change> Entries { get; }
  Maybe<RelativeDirectoryPath> LatestPackagePath();
  IEnumerable<Contribution> Contributions();
}

public class ImmutableFileHistory(
  RelativeFilePath pathOfCurrentVersion,
  double hotSpotRating,
  int changesCount,
  double complexityOfCurrentVersion,
  DateTimeOffset lastChangedDate,
  TimeSpan activityPeriod,
  IEnumerable<string> changeIds,
  DateTimeOffset creationDate,
  Maybe<RelativeDirectoryPath> latestPackagePath,
  TimeSpan timeSinceLastChange,
  TimeSpan age,
  IEnumerable<Contribution> contributions,
  IReadOnlyList<Change> entries)
  : IFileHistory
{
  private readonly HashSet<string> _changeIds = changeIds.ToHashSet();

  public RelativeFilePath PathOfCurrentVersion() => pathOfCurrentVersion;
  public double HotSpotRating() => hotSpotRating;
  public int ChangesCount() => changesCount;
  public double ComplexityOfCurrentVersion() => complexityOfCurrentVersion;
  public DateTimeOffset LastChangeDate() => lastChangedDate;
  public TimeSpan ActivityPeriod() => activityPeriod;
  public IEnumerable<string> ChangeIds() => _changeIds;
  public DateTimeOffset CreationDate() => creationDate;
  public TimeSpan TimeSinceLastChange() => timeSinceLastChange;
  public TimeSpan Age() => age;
  public IReadOnlyList<Change> Entries { get; } = entries;
  public IEnumerable<Contribution> Contributions() => contributions;
  public Maybe<RelativeDirectoryPath> LatestPackagePath() => latestPackagePath;

  public int CalculateCouplingCountTo(IFileHistory otherHistory)
  {
    if (otherHistory is ImmutableFileHistory immutableOtherHistory)
    {
      var smaller = _changeIds.Count <= immutableOtherHistory._changeIds.Count
        ? _changeIds
        : immutableOtherHistory._changeIds;
      var larger = ReferenceEquals(smaller, _changeIds)
        ? immutableOtherHistory._changeIds
        : _changeIds;
      var couplingCount = 0;
      foreach (var changeId in smaller)
      {
        if (larger.Contains(changeId))
        {
          couplingCount++;
        }
      }

      return couplingCount;
    }

    return _changeIds.Intersect(otherHistory.ChangeIds()).Count();
  }

  public CouplingBetweenFiles CalculateCouplingTo(
      IFileHistory otherHistory,
      int totalCommits,
      int couplingCount)
  {
    return new CouplingBetweenFiles(
        PathOfCurrentVersion(),
        otherHistory.PathOfCurrentVersion(),
        couplingCount,
        CouplingPercentages.CalculateUsing(ChangesCount(), otherHistory.ChangesCount(), couplingCount, totalCommits));
  }

  public IEnumerable<CouplingBetweenFiles> Filter(IEnumerable<CouplingBetweenFiles> couplingMetrics)
  {
    var couplingsLeft = couplingMetrics.Where(c => c.Left == PathOfCurrentVersion());
    var couplingsRight = couplingMetrics.Where(c => c.Right == PathOfCurrentVersion())
        .Select(CouplingWithSwitchedSides());
    return couplingsLeft.Concat(couplingsRight).OrderByDescending(c => c.CouplingCount);
  }

  private static Func<CouplingBetweenFiles, CouplingBetweenFiles> CouplingWithSwitchedSides()
  {
    return c => c.WithSwitchedSides();
  }
}
