using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic;

public class FlatPackageHistory(RelativeDirectoryPath packagePath) : IFlatPackageHistory
{
  private readonly List<IFileHistory> _files = new List<IFileHistory>();
  private readonly HashSet<string> _changeIds = new HashSet<string>();

  public void Add(IFileHistory fileHistory)
  {
    _files.Add(fileHistory);
    _changeIds.UnionWith(fileHistory.ChangeIds());
  }

  public IEnumerable<IFileHistory> Files => _files;
  public IEnumerable<string> ChangeIds()
  {
    return _changeIds;
  }

  //bug may be commonalized with IFileHistory
  public int CalculateCouplingCountTo(IFlatPackageHistory otherHistory)
  {
    if (otherHistory is FlatPackageHistory flatPackageHistory)
    {
      var smaller = _changeIds.Count <= flatPackageHistory._changeIds.Count
        ? _changeIds
        : flatPackageHistory._changeIds;
      var larger = ReferenceEquals(smaller, _changeIds)
        ? flatPackageHistory._changeIds
        : _changeIds;
      var couplingCount = 0;
      foreach (var changeId in smaller)
      {
        if (larger.Contains(changeId))
        {
          couplingCount++;
        }
      }

      return couplingCount;
    }

    return _changeIds.Intersect(otherHistory.ChangeIds()).Count();
  }

  public CouplingBetweenPackages CalculateCouplingTo(
      IFlatPackageHistory otherHistory,
      int totalCommits,
      int couplingCount)
  {
    return new CouplingBetweenPackages(
        packagePath,
        otherHistory.PathOfCurrentVersion(),
        couplingCount,
        CouplingPercentages.CalculateUsing(
            ChangesCount(),
            otherHistory.ChangesCount(),
            couplingCount,
            totalCommits));
  }

  public int ChangesCount()
  {
    return _changeIds.Count;
  }

  public double ComplexityOfCurrentVersion()
  {
    return _files.Sum(f => f.ComplexityOfCurrentVersion());
  }

  public double HotSpotRating()
  {
    //bug maybe calculate instead of just summing up

    return _files.Sum(f => f.HotSpotRating());
  }

  public RelativeDirectoryPath PathOfCurrentVersion()
  {
    return packagePath;
  }

  public DirectoryName Name()
  {
    return packagePath.DirectoryName();
  }

  public override string ToString()
  {
    return $"{nameof(packagePath)}: {packagePath}";
  }


}
