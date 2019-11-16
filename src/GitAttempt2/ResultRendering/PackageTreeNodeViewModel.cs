using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApplicationLogic;

namespace ResultRendering
{
  public class PackageTreeNodeViewModel
  {
    private readonly double _hotSpotRating;

    public PackageTreeNodeViewModel(
        double hotSpotRating, 
        string pathOfCurrentVersion)
    {
      Name = Path.GetFileName(pathOfCurrentVersion);
      _hotSpotRating = hotSpotRating;
    }

    public List<PackageTreeNodeViewModel> Children { get; } = new List<PackageTreeNodeViewModel>();
    public PackageTreeNodeViewModel Parent { get; set; }
    public string Name { get; }

    public double HotSpotRating
    {
      get { return _hotSpotRating + Children.Sum(c => c.HotSpotRating); }
    }

    public static PackageTreeNodeViewModel From(PackageHistoryNode packageTree)
    {
      var packageNodeViewModelVisitor = new PackageNodeViewModelVisitor();
      packageTree.Accept(packageNodeViewModelVisitor);
      return packageNodeViewModelVisitor.ToPackageNodeViewModel();
    }
  }
}