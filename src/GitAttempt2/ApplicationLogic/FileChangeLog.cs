using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public interface IFileChangeLog : IChangeLog
  {
    DateTimeOffset CreationDate();
    DateTimeOffset LastChangeDate();
    TimeSpan ActivityPeriod();
    TimeSpan TimeSinceLastChange();
    void AssignChangeCountRank(int rank);
    void AssignComplexityRank(int rank);
    IEnumerable<string> ChangeIds();
  }

  public class FileChangeLog : IFileChangeLog
  {
        private readonly List<Change> _entries = new List<Change>();
        private int _complexityRank = -1; //bug handle another way
        private int _changeCountRank = -1; //bug handle another way
        private readonly IClock _clock;

        public FileChangeLog(IClock clock)
        {
          _clock = clock;
        }

        public IReadOnlyList<Change> Entries => _entries;

        public void AddDataFrom(Change change)
        {
            _entries.Add(change);
        }

        public string PathOfCurrentVersion() => Entries.Last().Path;
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

        public string PackagePath()
        {
          return System.IO.Path.GetDirectoryName(PathOfCurrentVersion());
        }

        public Coupling CalculateCouplingTo(FileChangeLog otherChangeLog)
        {
          var couplingCount = 0;
          foreach (var change in _entries)
          {
            if (otherChangeLog.WasChangedIn(change.Id))
            {
              couplingCount++;
            }
          }
          return new Coupling(PathOfCurrentVersion(), otherChangeLog.PathOfCurrentVersion(), couplingCount);
        }

        private bool WasChangedIn(string changeId)
        {
          return _entries.Select(e => e.Id).Contains(changeId);
        }

        public IEnumerable<Coupling> Filter(IEnumerable<Coupling> couplingMetrics)
        {
          var couplingsLeft = couplingMetrics.Where(c => c.Left == PathOfCurrentVersion());
          var couplingsRight = couplingMetrics.Where(c => c.Right == PathOfCurrentVersion())
            .Select(c => new Coupling(c.Right, c.Left, c.CouplingCount));
          return couplingsLeft.Concat(couplingsRight);
        }
  }
}