using System.Collections.Generic;
using System.Linq;
using ApplicationLogic;
using AtmaFileSystem;

static internal class PackageHistoryNodeFactory
{
  public static PackageHistoryNode NewPackageNode(IFlatPackageHistory packageHistory)
  {
    return new PackageHistoryNode(
      packageHistory, 
      packageHistory.Files.Select(
        f => new FileHistoryNode(f)));
  }

  public static PackagesTree NewPackagesTree()
  {
    return new PackagesTree();
  }
}