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

        public int ChangesCount() => Entries.Count;
        public double ComplexityOfCurrentVersion() => Entries.Last().Complexity;

        public ImmutableFileHistory ToImmutableFileHistory()
        {
            return new ImmutableFileHistory(
                PathOfCurrentVersion(),
                HotSpotRating(),
                ChangesCount(),
                ComplexityOfCurrentVersion(),
                LastChangeDate(),
                ActivityPeriod(),
                ChangeIds(),
                CreationDate(),
                LatestPackagePath(),
                _clock.Now() - LastChangeDate(),
                _clock.Now() - CreationDate(),
                _entries);
        }

        private RelativeFilePath PathOfCurrentVersion() => Entries.Last().Path;
        private IReadOnlyList<Change> Entries => _entries;
        private IEnumerable<string> ChangeIds() => _entries.Select(e => e.Id);
        private Maybe<RelativeDirectoryPath> LatestPackagePath() => PathOfCurrentVersion().ParentDirectory();
        private double HotSpotRating() => ComplexityMetrics.CalculateHotSpotRating(_complexityRank.Value, _changeCountRank.Value);
        private DateTimeOffset CreationDate() => Entries.First().ChangeDate;
        private DateTimeOffset LastChangeDate() => Entries.Last().ChangeDate;
        private TimeSpan ActivityPeriod() => LastChangeDate() - CreationDate();
  }
}