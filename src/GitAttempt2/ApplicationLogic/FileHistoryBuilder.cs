using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;
using Functional.Maybe.Just;

namespace ApplicationLogic
{

  public interface IFileHistoryBuilder
  {
      void AssignChangeCountRank(int rank);
      void AssignComplexityRank(int rank);
      int ChangesCount();
      double ComplexityOfCurrentVersion();
  }

  public class FileHistoryBuilder : IFileHistoryBuilder
  {
        private readonly List<Change> _entries = new List<Change>();
        private Maybe<int> _complexityRank = Maybe<int>.Nothing;
        private Maybe<int> _changeCountRank = Maybe<int>.Nothing;
        private readonly IClock _clock;

        public FileHistoryBuilder(IClock clock)
        {
            _clock = clock;
        }

        public void AddDataFrom(Change change)
        {
            _entries.Add(change);
        }

        public void AssignComplexityRank(int complexityRank)
        {
          _complexityRank = complexityRank.Just();
        }

        public void AssignChangeCountRank(int changeCountRank)
        {
          _changeCountRank = changeCountRank.Just();
        }

        public int ChangesCount() => _entries.Count;
        public double ComplexityOfCurrentVersion() => _entries.Last().Complexity.Value;

        public IFileHistory ToImmutableFileHistory()
        {
            return new ImmutableFileHistory(
                _entries.Last().Path,
                ComplexityMetrics.CalculateHotSpotRating(_complexityRank.Value, _changeCountRank.Value),
                ChangesCount(),
                ComplexityOfCurrentVersion(),
                _entries.Last().ChangeDate,
                _entries.Last().ChangeDate - _entries.First().ChangeDate,
                _entries.Select(e => e.Id),
                _entries.First().ChangeDate,
                _entries.Last().Path.ParentDirectory(),
                _clock.Now() - _entries.Last().ChangeDate,
                _clock.Now() - _entries.First().ChangeDate,
                _entries);
        }
  }
}