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
        IEnumerable<string> ChangeIds();
        Coupling CalculateCouplingTo(IFileHistory otherHistory, int totalCommits);
        IEnumerable<Coupling> Filter(IEnumerable<Coupling> couplingMetrics);
        DateTimeOffset CreationDate();
        TimeSpan TimeSinceLastChange();
        TimeSpan Age();
        IReadOnlyList<Change> Entries { get; }
        Maybe<RelativeDirectoryPath> LatestPackagePath();
    }

    public class ImmutableFileHistory : IFileHistory
    {
        private readonly RelativeFilePath _pathOfCurrentVersion;
        private readonly double _hotSpotRating;
        private readonly int _changesCount;
        private readonly double _complexityOfCurrentVersion;
        private readonly DateTimeOffset _lastChangedDate;
        private readonly TimeSpan _activityPeriod;
        private readonly IEnumerable<string> _changeIds;
        private readonly DateTimeOffset _creationDate;
        private readonly Maybe<RelativeDirectoryPath> _latestPackagePath;
        private readonly TimeSpan _timeSinceLastChange;
        private readonly TimeSpan _age;

        public ImmutableFileHistory(
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
            IReadOnlyList<Change> entries)
        {
            _pathOfCurrentVersion = pathOfCurrentVersion;
            _hotSpotRating = hotSpotRating;
            _changesCount = changesCount;
            _complexityOfCurrentVersion = complexityOfCurrentVersion;
            _lastChangedDate = lastChangedDate;
            _activityPeriod = activityPeriod;
            _changeIds = changeIds;
            _creationDate = creationDate;
            _latestPackagePath = latestPackagePath;
            _timeSinceLastChange = timeSinceLastChange;
            _age = age;
            Entries = entries;
        }

        public RelativeFilePath PathOfCurrentVersion()
        {
            return _pathOfCurrentVersion;
        }

        public double HotSpotRating()
        {
            return _hotSpotRating;
        }

        public int ChangesCount()
        {
            return _changesCount;
        }

        public double ComplexityOfCurrentVersion()
        {
            return _complexityOfCurrentVersion;
        }

        public DateTimeOffset LastChangeDate()
        {
            return _lastChangedDate;
        }

        public TimeSpan ActivityPeriod()
        {
            return _activityPeriod;
        }

        public IEnumerable<string> ChangeIds()
        {
            return _changeIds;
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

        public DateTimeOffset CreationDate()
        {
            return _creationDate;
        }

        public TimeSpan TimeSinceLastChange()
        {
            return _timeSinceLastChange;
        }

        public TimeSpan Age()
        {
            return _age;
        }

        public IReadOnlyList<Change> Entries { get; }
        public Maybe<RelativeDirectoryPath> LatestPackagePath() => _latestPackagePath;
    }
}