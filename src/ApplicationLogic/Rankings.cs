using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;
using Functional.Maybe.Just;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NHotSpot.ApplicationLogic
{
  public class PackagesTree
  {
    private Dictionary<RelativeDirectoryPath, IPackageHistoryNode> NodeCache { get; } =
      new Dictionary<RelativeDirectoryPath, IPackageHistoryNode>();

    private void Set(RelativeDirectoryPath path, IPackageHistoryNode newNode)
    {
      NodeCache[path] = newNode;
    }

    private void BindWithParent(RelativeDirectoryPath path, PackageHistoryNode newNode)
    {
      while (true)
      {
        var parentPath = path.ParentDirectory();
        if (parentPath.HasValue)
        {
          if (NodeCache.ContainsKey(parentPath.Value))
          {
            NodeCache[parentPath.Value].AddChild(newNode);
            break;
          }

          path = parentPath.Value;
        }
        else
        {
          break;
        }
      }
    }

    public void Add(RelativeDirectoryPath path, PackageHistoryNode newNode)
    {
      Set(path, newNode);
      BindWithParent(path, newNode);
    }

    public IPackageHistoryNode Root()
    {
      var potentialRoots = NodeCache.Values.Where(n => !n.HasParent()).ToArray();
      if (!potentialRoots.Any())
      {
        return new NoFilesOrPackages();
      }
      if (potentialRoots.Count() > 1)
      {
        throw new Exception($"Detected {potentialRoots.Count()} potential roots. Programmer error");
      }

      return potentialRoots.Single();
    }
  }

  public static class Rankings
  {
    private static readonly RelativeDirectoryPath ArtificialRoot;

    static Rankings()
    {
      ArtificialRoot = RelativeDirectoryPath("ROOT");
    }

    public static Dictionary<RelativeDirectoryPath, IFlatPackageHistory> GatherFlatPackageHistoriesByPath(IEnumerable<IFileHistory> fileChangeLogs)
    {
      var packageHistoriesByPath = new Dictionary<RelativeDirectoryPath, IFlatPackageHistory>();
      foreach (var fileHistory in fileChangeLogs)
      {
        var packagePath = fileHistory.LatestPackagePath().Select(p => ArtificialRoot + p).OrElse(ArtificialRoot);

        EnsurePathIsIn(packageHistoriesByPath, packagePath);
        packageHistoriesByPath[packagePath].Add(fileHistory);

        var currentPackagePath = packagePath.Just();
        do
        {
          EnsurePathIsIn(packageHistoriesByPath, currentPackagePath.Value);
          currentPackagePath = currentPackagePath.Value.ParentDirectory();
        } while (currentPackagePath.HasValue);
      }

      return packageHistoriesByPath;
    }

    private static void EnsurePathIsIn(IDictionary<RelativeDirectoryPath, IFlatPackageHistory> packageChangeLogsByPath, RelativeDirectoryPath packagePath)
    {
      if (!packageChangeLogsByPath.ContainsKey(packagePath))
      {
        packageChangeLogsByPath[packagePath] = new FlatPackageHistory(packagePath);
      }
    }

    public static IPackageHistoryNode GatherPackageTreeMetricsByPath(IEnumerable<IFileHistory> fileChangeLogs)
    {
      var packagesTree = PackageTreeFactory.NewPackagesTree();
      var flatPackageMetricsByPath = GatherFlatPackageHistoriesByPath(fileChangeLogs);
      foreach (var (path, packageHistory) in flatPackageMetricsByPath.ToList().OrderBy(kvp => kvp.Key))
      {
        packagesTree.Add(path, PackageTreeFactory.NewPackageNode(packageHistory));
      }

      return packagesTree.Root();
    }


    private static Func<TEntry, int, (TEntry entry, int index)> WithIndex<TEntry>()
    {
      return (entry, index) => (entry, index: index + 1);
    }


    public static void UpdateChangeCountRankingBasedOnOrderOf(IEnumerable<IFileHistoryBuilder> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IFileHistoryBuilder>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignChangeCountRank(tuple.index));
    }

    public static void UpdateComplexityRankingBasedOnOrderOf(IEnumerable<IFileHistoryBuilder> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IFileHistoryBuilder>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignComplexityRank(tuple.index));
    }

  }

  static class KvpExtensions
  {
    public static void Deconstruct<TKey, TValue>(
      this KeyValuePair<TKey, TValue> kvp,
      out TKey key,
      out TValue value)
    {
      key = kvp.Key;
      value = kvp.Value;
    }
  }
}