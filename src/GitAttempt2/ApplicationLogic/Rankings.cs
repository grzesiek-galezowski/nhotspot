using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;

namespace ApplicationLogic
{
  public static class Rankings
  {
    public static Dictionary<string, IFlatPackageChangeLog> GatherFlatPackageMetricsByPath(IEnumerable<FileChangeLog> fileChangeLogs)
    {
      var packageChangeLogsByPath = new Dictionary<string, IFlatPackageChangeLog>();
      foreach (var fileChangeLog in fileChangeLogs)
      {
        string packagePath = fileChangeLog.PackagePath();
        EnsurePathIsIn(packageChangeLogsByPath, packagePath);
        packageChangeLogsByPath[packagePath].Add(fileChangeLog);

        do
        {
          EnsurePathIsIn(packageChangeLogsByPath, packagePath);
          packagePath = Path.GetDirectoryName(packagePath);
        } while (packagePath != null);
      }

      return packageChangeLogsByPath;
    }

    private static void EnsurePathIsIn(Dictionary<string, IFlatPackageChangeLog> packageChangeLogsByPath, string packagePath)
    {
      if (!packageChangeLogsByPath.ContainsKey(packagePath))
      {
        packageChangeLogsByPath[packagePath] = new FlatPackageChangeLog(packagePath);
      }
    }

    public static PackageChangeLogNode GatherPackageTreeMetricsByPath(IEnumerable<FileChangeLog> fileChangeLogs)
    {
      var nodes = new Dictionary<string, PackageChangeLogNode>();
      var flatPackageMetricsByPath = GatherFlatPackageMetricsByPath(fileChangeLogs);
      foreach (var packageChangeLogEntry in flatPackageMetricsByPath.ToList().OrderBy(kvp => kvp.Key))
      {
        var path = packageChangeLogEntry.Key;
        var packageChangeLog = packageChangeLogEntry.Value;

        var newNode = NewPackageNode(packageChangeLog);
        nodes[path] = newNode;

        AddToParent(nodes, newNode, path);
      }

      return nodes.Values.Single(n => !n.HasParent());
    }

    private static PackageChangeLogNode NewPackageNode(IFlatPackageChangeLog packageChangeLog)
    {
        return new PackageChangeLogNode(
            packageChangeLog, 
            packageChangeLog.Files.Select(
                f => new FileChangeLogNode(f)));
    }


    ///src/csharp
    private static void AddToParent(Dictionary<string, PackageChangeLogNode> nodes, PackageChangeLogNode newNode, string path)
    {
      var parentPath = Path.GetDirectoryName(path).ToMaybe();
      if (parentPath.HasValue)
      {
        if (nodes.ContainsKey(parentPath.Value))
        {
          nodes[parentPath.Value].AddChild(newNode);
        }
        else
        {
          AddToParent(nodes, newNode, parentPath.Value);
        }
      }
    }

    private static Func<TEntry, int, (TEntry entry, int index)> WithIndex<TEntry>()
    {
      return (entry, index) => (entry, index: index + 1);
    }


    public static void UpdateChangeCountRankingBasedOnOrderOf(IEnumerable<IFileChangeLog> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IFileChangeLog>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignChangeCountRank(tuple.index));
    }

    public static void UpdateComplexityRankingBasedOnOrderOf(IEnumerable<IFileChangeLog> entriesToRank)
    {
      entriesToRank
        .Select(WithIndex<IFileChangeLog>())
        .ToList().ForEach(
          tuple => tuple.entry.AssignComplexityRank(tuple.index));
    }

  }

  public class FileChangeLogNode
  {
    private readonly IFileChangeLog _fileChangeLog;

    public FileChangeLogNode(IFileChangeLog fileChangeLog)
    {
      _fileChangeLog = fileChangeLog;
    }

    public void Accept(INodeVisitor visitor)
    {
      visitor.Visit(_fileChangeLog);
    }
  }
}