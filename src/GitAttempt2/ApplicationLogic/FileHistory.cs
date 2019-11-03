using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;

namespace ApplicationLogic
{
  public interface IFileHistory : IItemHistory<RelativeFilePath>
  {
    DateTimeOffset LastChangeDate();
    TimeSpan ActivityPeriod();
    void AssignChangeCountRank(int rank);
    void AssignComplexityRank(int rank);
    IEnumerable<string> ChangeIds();
    Coupling CalculateCouplingTo(IFileHistory otherHistory, int totalCommits);
  }

  public class FileHistory : IFileHistory
  {
        private readonly List<Change> _entries = new List<Change>();
        private int _complexityRank = -1; //bug handle another way
        private int _changeCountRank = -1; //bug handle another way
        private readonly IClock _clock;

        public FileHistory(IClock clock)
        {
          _clock = clock;
        }

        public IReadOnlyList<Change> Entries => _entries;

        public void AddDataFrom(Change change)
        {
            _entries.Add(change);
        }

        public RelativeFilePath PathOfCurrentVersion() => RelativeFilePath.Value(Entries.Last().Path);
        public int ChangesCount() => Entries.Count;
        public double ComplexityOfCurrentVersion() => Entries.Last().Complexity;
        public double HotSpotRating() => ComplexityMetrics.CalculateHotSpotRating(_complexityRank, _changeCountRank);

        public DateTimeOffset CreationDate() => Entries.First().ChangeDate;
        public DateTimeOffset LastChangeDate() => Entries.Last().ChangeDate;
        public TimeSpan ActivityPeriod() => LastChangeDate() - CreationDate();
        public TimeSpan Age() => _clock.Now() - CreationDate();
        public TimeSpan TimeSinceLastChange() => _clock.Now() - LastChangeDate();

        public void AssignComplexityRank(int complexityRank)
        {
          _complexityRank = complexityRank;
        }

        public IEnumerable<string> ChangeIds()
        {
          return _entries.Select(e => e.Id);
        }

        public void AssignChangeCountRank(int changeCountRank)
        {
          _changeCountRank = changeCountRank;
        }

        public Maybe<RelativeDirectoryPath> LatestPackagePath()
        {
          return PathOfCurrentVersion().ParentDirectory();
        }

        public Coupling CalculateCouplingTo(IFileHistory otherHistory, int totalCommits)
        {
          //performance-critical fragment
          var couplingCount = ChangeIds().Intersect(otherHistory.ChangeIds()).Count();
          return new Coupling(
            PathOfCurrentVersion(), 
            otherHistory.PathOfCurrentVersion(), 
            couplingCount,
            (couplingCount * 100)/ChangesCount(),
            (couplingCount * 100)/otherHistory.ChangesCount(),
            (couplingCount * 100)/totalCommits
            );
        }

        public IEnumerable<Coupling> Filter(IEnumerable<Coupling> couplingMetrics)
        {
          var couplingsLeft = couplingMetrics.Where(c => c.Left == PathOfCurrentVersion());
          var couplingsRight = couplingMetrics.Where(c => c.Right == PathOfCurrentVersion())
            .Select(CouplingWithSwitchedSides());
          return couplingsLeft.Concat(couplingsRight).OrderByDescending(c => c.CouplingCount);
        }

        private static Func<Coupling, Coupling> CouplingWithSwitchedSides()
        {
          return c => new Coupling(
            c.Right, 
            c.Left, 
            c.CouplingCount, 
            c.PercentageOfRightCommits, 
            c.PercentageOfLeftCommits,
            c.PercentageOfTotalCommits);
        }
  }
}