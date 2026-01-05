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
  public RelativeFilePath PathOfCurrentVersion() => pathOfCurrentVersion;
  public double HotSpotRating() => hotSpotRating;
  public int ChangesCount() => changesCount;
  public double ComplexityOfCurrentVersion() => complexityOfCurrentVersion;
  public DateTimeOffset LastChangeDate() => lastChangedDate;
  public TimeSpan ActivityPeriod() => activityPeriod;
  public IEnumerable<string> ChangeIds() => changeIds;
  public DateTimeOffset CreationDate() => creationDate;
  public TimeSpan TimeSinceLastChange() => timeSinceLastChange;
  public TimeSpan Age() => age;
  public IReadOnlyList<Change> Entries { get; } = entries;
  public IEnumerable<Contribution> Contributions() => contributions;
  public Maybe<RelativeDirectoryPath> LatestPackagePath() => latestPackagePath;

  public CouplingBetweenFiles CalculateCouplingTo(
      IFileHistory otherHistory,
      int totalCommits)
  {
    //performance-critical fragment
    var couplingCount = ChangeIds().Intersect(otherHistory.ChangeIds()).Count();
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
