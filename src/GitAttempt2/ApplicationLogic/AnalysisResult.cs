using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public class AnalysisResult
  {
    public string Path { get; }

    public PackageChangeLogNode PackageTree()
    {
      return _packageChangeLogNode;
    }

    private readonly IEnumerable<FileChangeLog> _changeLogs;

    private readonly Dictionary<string, IFlatPackageChangeLog> _packageChangeLogsByPath;
    private readonly PackageChangeLogNode _packageChangeLogNode;

    public AnalysisResult(IEnumerable<FileChangeLog> changeLogs,
      Dictionary<string, IFlatPackageChangeLog> packageMetricsByPath,
      string normalizedPath, 
      PackageChangeLogNode packageChangeLogNode)
    {
      Path = normalizedPath;
      _changeLogs = changeLogs;
      _packageChangeLogsByPath = packageMetricsByPath;
      _packageChangeLogNode = packageChangeLogNode;
    }

    public IEnumerable<FileChangeLog> EntriesByHotSpotRating()
    {
      return _changeLogs.OrderByDescending(h => h.HotSpotRating());
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

    public IEnumerable<IFlatPackageChangeLog> PackagesByDiminishingHotSpotRating()
    {
      return _packageChangeLogsByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRating());
    }

    
  }
}