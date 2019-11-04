using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace ApplicationLogic
{
  public class PackagesTree
  {
    private Dictionary<RelativeDirectoryPath, PackageHistoryNode> NodeCache { get; } =
      new Dictionary<RelativeDirectoryPath, PackageHistoryNode>();

    private void Set(RelativeDirectoryPath path, PackageHistoryNode newNode)
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

    public PackageHistoryNode Root()
    {
      return NodeCache.Values.Single(n => !n.HasParent());
    }
  }

  public static class Rankings
  {
    private static readonly RelativeDirectoryPath ArtificialRoot;

    static Rankings()
    {
      ArtificialRoot = RelativeDirectoryPath(".");
    }

    public static Dictionary<RelativeDirectoryPath, IFlatPackageHistory> GatherFlatPackageHistoriesByPath(IEnumerable<IFileHistory> fileChangeLogs)
    {
      var packageHistoriesByPath = new Dictionary<RelativeDirectoryPath, IFlatPackageHistory>();
      foreach (var fileHistory in fileChangeLogs)
      {
        var packagePath = fileHistory.LatestPackagePath().Select(p => ArtificialRoot + p).OrElse(ArtificialRoot);

        EnsurePathIsIn(packageHistoriesByPath, packagePath);
        packageHistoriesByPath[packagePath].Add(fileHistory);

        var packagePath2 = packagePath.ToMaybe();
        do
        {
          EnsurePathIsIn(packageHistoriesByPath, packagePath2.Value);
          packagePath2 = packagePath2.Value.ParentDirectory();
        } while (packagePath2.HasValue);
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

    public static PackageHistoryNode GatherPackageTreeMetricsByPath(IEnumerable<IFileHistory> fileChangeLogs)
    {
      var packagesTree = PackageHistoryNodeFactory.NewPackagesTree();
      var flatPackageMetricsByPath = GatherFlatPackageHistoriesByPath(fileChangeLogs);
      foreach (var (path, packageHistory) in flatPackageMetricsByPath.ToList().OrderBy(kvp => kvp.Key))
      {
        packagesTree.Add(path, PackageHistoryNodeFactory.NewPackageNode(packageHistory));
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