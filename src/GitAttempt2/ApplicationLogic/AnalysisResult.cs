using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtmaFileSystem;

namespace ApplicationLogic
{
  public class AnalysisResult
  {
    private readonly IEnumerable<FileHistory> _changeLogs;
    private readonly Dictionary<RelativeDirectoryPath, IFlatPackageHistory> _packageHistoriesByPath;
    private readonly PackageHistoryNode _packageHistoryNode;
    private readonly IEnumerable<Coupling> _changeCouplings;

    public AnalysisResult(IEnumerable<FileHistory> changeLogs,
      Dictionary<RelativeDirectoryPath, IFlatPackageHistory> packageHistoriesByPath,
      string normalizedPathToRepository, 
      PackageHistoryNode packageHistoryNode, 
      IEnumerable<Coupling> changeCouplings)
    {
      PathToRepository = normalizedPathToRepository;
      _changeLogs = changeLogs;
      _packageHistoriesByPath = packageHistoriesByPath;
      _packageHistoryNode = packageHistoryNode;
      _changeCouplings = changeCouplings;
    }

    public string PathToRepository { get; }

    public PackageHistoryNode PackageTree()
    {
      return _packageHistoryNode;
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

    public IEnumerable<IFlatPackageHistory> PackagesByDiminishingHotSpotRating()
    {
      return _packageHistoriesByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRating());
    }

    
  }
}