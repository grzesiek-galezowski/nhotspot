using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace ApplicationLogic
{
  public class AnalysisResult
  {
    private readonly IPackageHistoryNode _packageHistoryRootNode;
    private readonly IEnumerable<Coupling> _changeCouplings;
    private readonly IOrderedEnumerable<IFileHistory> _entriesByHotSpotRating;
    private readonly IEnumerable<IFileHistory> _entriesByDiminishingComplexity;
    private readonly IOrderedEnumerable<IFileHistory> _entriesByDiminishingChangesCount;
    private readonly IOrderedEnumerable<IFileHistory> _entriesByDiminishingActivityPeriod;
    private readonly IOrderedEnumerable<IFileHistory> _entriesFromMostRecentlyChanged;
    private readonly IEnumerable<IFileHistory> _entriesFromMostAncientlyChanged;
    private readonly IOrderedEnumerable<IFlatPackageHistory> _packagesByDiminishingHotSpotRating;

    public AnalysisResult(IEnumerable<IFileHistory> fileHistories,
      Dictionary<RelativeDirectoryPath, IFlatPackageHistory> packageHistoriesByPath,
      string normalizedPathToRepository, 
      IPackageHistoryNode packageHistoryRootNode, 
      IEnumerable<Coupling> changeCouplings)
    {
      PathToRepository = normalizedPathToRepository;
      _packageHistoryRootNode = packageHistoryRootNode;
      _changeCouplings = changeCouplings;
      _entriesByHotSpotRating = fileHistories.OrderByDescending(h => h.HotSpotRating());
      _entriesByDiminishingComplexity = fileHistories.OrderByDescending(h => h.ComplexityOfCurrentVersion());
      _entriesByDiminishingChangesCount = fileHistories.OrderByDescending(h => h.ChangesCount());
      _entriesByDiminishingActivityPeriod = fileHistories.OrderByDescending(h => h.ActivityPeriod());
      _entriesFromMostRecentlyChanged = fileHistories.OrderByDescending(h => h.LastChangeDate());
      _entriesFromMostAncientlyChanged = _entriesFromMostRecentlyChanged.Reverse();
      _packagesByDiminishingHotSpotRating = packageHistoriesByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRating());
    }

    public string PathToRepository { get; }

    public IPackageHistoryNode PackageTree()
    {
      return _packageHistoryRootNode;
    }

    public IEnumerable<Coupling> CouplingMetrics()
    {
      return _changeCouplings;
    }

    public IEnumerable<IFileHistory> EntriesByHotSpotRating()
    {
      return _entriesByHotSpotRating;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingComplexity()
    {
      return _entriesByDiminishingComplexity;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingChangesCount()
    {
      return _entriesByDiminishingChangesCount;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingActivityPeriod()
    {
      return _entriesByDiminishingActivityPeriod;
    }

    public IEnumerable<IFileHistory> EntriesFromMostRecentlyChanged()
    {
      return _entriesFromMostRecentlyChanged;
    }

    public IEnumerable<IFileHistory> EntriesFromMostAncientlyChanged()
    {
      return _entriesFromMostAncientlyChanged;
    }

    public IEnumerable<IFlatPackageHistory> PackagesByDiminishingHotSpotRating()
    {
      return _packagesByDiminishingHotSpotRating;
    }

    
  }
}