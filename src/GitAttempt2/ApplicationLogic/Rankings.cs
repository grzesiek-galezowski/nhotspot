using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public static class Rankings
  {
    public static Dictionary<string, IPackageChangeLog> GatherPackageMetricsByPath(
      IEnumerable<FileChangeLog> fileChangeLogs) //bug make this a function
    {
      var packageChangeLogsByPath = new Dictionary<string, IPackageChangeLog>();
      foreach (var fileChangeLog in fileChangeLogs)
      {
        string packagePath = fileChangeLog.PackagePath();
        if (!packageChangeLogsByPath.ContainsKey(packagePath))
        {
          packageChangeLogsByPath[packagePath] = new PackageChangeLog(packagePath);
        }

        packageChangeLogsByPath[packagePath].AddMetricsFrom(fileChangeLog);
      }

      UpdateChangeCountRankingBasedOnOrderOf(packageChangeLogsByPath.Values.OrderBy(v => v.ChangesCount()));
      UpdateComplexityRankingBasedOnOrderOf(
        packageChangeLogsByPath.Values.OrderBy(v => v.ComplexityOfCurrentVersion()));


      return packageChangeLogsByPath;
    }

    private static Func<TEntry, int, (TEntry entry, int index)> WithIndex<TEntry>()
    {
      return (entry, index) => (entry, index: index + 1);
    }


    public static void UpdateChangeCountRankingBasedOnOrderOf(IEnumerable<IChangeLog> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IChangeLog>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignChangeCountRank(tuple.index));
    }

    public static void UpdateComplexityRankingBasedOnOrderOf(IEnumerable<IChangeLog> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IChangeLog>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignComplexityRank(tuple.index));
    }

  }
}