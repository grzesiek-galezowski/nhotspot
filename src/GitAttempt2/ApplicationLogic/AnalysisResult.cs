using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public class AnalysisResult
  {
    public string Path { get; }
    private readonly IEnumerable<FileChangeLog> _changeLogs;

    private readonly Dictionary<string, IPackageChangeLog> _packageChangeLogsByPath = new Dictionary<string, IPackageChangeLog>();

    public AnalysisResult(
      IEnumerable<FileChangeLog> changeLogs, 
      Dictionary<string, IPackageChangeLog> packageMetricsByPath, 
      string normalizedPath)
    {
      Path = normalizedPath;
      _changeLogs = changeLogs;
      _packageChangeLogsByPath = packageMetricsByPath;
    }

    public IEnumerable<FileChangeLog> EntriesByHotSpotRank()
    {
      return _changeLogs.OrderByDescending(h => h.HotSpotRank());
    }

    private IEnumerable<IFileChangeLog> EntriesByRisingComplexity()
    {
      return _changeLogs.OrderBy(h => h.ComplexityOfCurrentVersion());
    }

    public IEnumerable<IFileChangeLog> EntriesByDiminishingComplexity()
    {
      return EntriesByRisingComplexity().Reverse();
    }

    public IEnumerable<IFileChangeLog> EntriesByRisingChangesCount()
    {
      return _changeLogs.OrderBy(h => h.ChangesCount());
    }

    public IEnumerable<IFileChangeLog> EntriesByDiminishingChangesCount()
    {
      return _changeLogs.OrderByDescending(h => h.ChangesCount());
    }

    public IEnumerable<IFileChangeLog> EntriesByDiminishingActivityPeriod()
    {
      return _changeLogs.OrderByDescending(h => h.ActivityPeriod());
    }

    public IEnumerable<IFileChangeLog> EntriesFromMostRecentlyChanged()
    {
      return _changeLogs.OrderByDescending(h => h.LastChangeDate());
    }
    public IEnumerable<IFileChangeLog> EntriesFromMostAncientlyChanged()
    {
      return EntriesFromMostRecentlyChanged().Reverse();
    }

    public IEnumerable<IPackageChangeLog> PackagesByDiminishingHotSpotRank()
    {
      return _packageChangeLogsByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRank());
    }
  }
}