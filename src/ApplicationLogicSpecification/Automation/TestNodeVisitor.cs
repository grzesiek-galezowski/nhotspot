using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification.Automation;

public class TestNodeVisitor : INodeVisitor
{
    public readonly List<(int nesting, IFlatPackageHistory history)> OrderedPackages 
        = new List<(int, IFlatPackageHistory)>();
    private int _nesting = 0;
    public readonly Dictionary<RelativeDirectoryPath, IFlatPackageHistory> PackagesByPath 
        = new Dictionary<RelativeDirectoryPath, IFlatPackageHistory>();
    public readonly Dictionary<RelativeDirectoryPath, List<IFileHistory>> FilesByPackage 
        = new Dictionary<RelativeDirectoryPath, List<IFileHistory>>();

    public void BeginVisiting(IFlatPackageHistory value)
    {
        _nesting++;
        OrderedPackages.Add((_nesting, value));
        PackagesByPath[value.PathOfCurrentVersion()] = value;
    }

    public void EndVisiting(IFlatPackageHistory value)
    {
        _nesting--;
    }

    public void Visit(IFileHistory fileHistory)
    {
        var key = OrderedPackages.Last().history.PathOfCurrentVersion();
        if (!FilesByPackage.ContainsKey(key))
        {
            FilesByPackage[key] = new List<IFileHistory>();
        }
        FilesByPackage[key].Add(fileHistory);
    }
}