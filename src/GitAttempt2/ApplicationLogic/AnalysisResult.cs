using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogic
{
  public class AnalysisResult
  {
    public string Path { get; }

    public PackageChangeLogNode PackageTree()
    {
      return _packageChangeLogNode;
    }

    private readonly IEnumerable<FileHistory> _changeLogs;

    private readonly Dictionary<string, IFlatPackageChangeLog> _packageChangeLogsByPath;
    private readonly PackageChangeLogNode _packageChangeLogNode;
    private readonly IEnumerable<Coupling> _changeCouplings;

    public AnalysisResult(IEnumerable<FileHistory> changeLogs,
      Dictionary<string, IFlatPackageChangeLog> packageMetricsByPath,
      string normalizedPath, 
      PackageChangeLogNode packageChangeLogNode, 
      IEnumerable<Coupling> changeCouplings)
    {
      Path = normalizedPath;
      _changeLogs = changeLogs;
      _packageChangeLogsByPath = packageMetricsByPath;
      _packageChangeLogNode = packageChangeLogNode;
      _changeCouplings = changeCouplings;
    }

    public IEnumerable<Coupling> CouplingMetrics()
    {
      return _changeCouplings;
    }

    public IEnumerable<FileHistory> EntriesByHotSpotRating()
    {
      return _changeLogs.OrderByDescending(h => h.HotSpotRating());
    }

    private IEnumerable<IFileHistory> EntriesByRisingComplexity()
    {
      return _changeLogs.OrderBy(h => h.ComplexityOfCurrentVersion());
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingComplexity()
    {
      return EntriesByRisingComplexity().Reverse();
    }

    public IEnumerable<IFileHistory> EntriesByRisingChangesCount()
    {
      return _changeLogs.OrderBy(h => h.ChangesCount());
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingChangesCount()
    {
      return _changeLogs.OrderByDescending(h => h.ChangesCount());
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingActivityPeriod()
    {
      return _changeLogs.OrderByDescending(h => h.ActivityPeriod());
    }

    public IEnumerable<IFileHistory> EntriesFromMostRecentlyChanged()
    {
      return _changeLogs.OrderByDescending(h => h.LastChangeDate());
    }
    public IEnumerable<IFileHistory> EntriesFromMostAncientlyChanged()
    {
      return EntriesFromMostRecentlyChanged().Reverse();
    }

    public IEnumerable<IFlatPackageChangeLog> PackagesByDiminishingHotSpotRating()
    {
      return _packageChangeLogsByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRating());
    }

    
  }
}