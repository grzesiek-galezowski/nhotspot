using System.Linq;

namespace ApplicationLogic
{
  public static class PackageHistoryNodeFactory
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
}