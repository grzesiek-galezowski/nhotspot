using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using NHotSpot.ApplicationLogic;
using NullableReferenceTypesExtensions;

namespace NHotSpot.ResultRendering
{
  public class PackageTreeNodeViewModel
  {
    private readonly double _hotSpotRating;

    public PackageTreeNodeViewModel(
      double hotSpotRating,
      string pathOfCurrentVersion, 
      Maybe<PackageTreeNodeViewModel> parent)
    {
      Name = Path.GetFileName(pathOfCurrentVersion).OrThrow();
      _hotSpotRating = hotSpotRating;
      Parent = parent;
    }

    public List<PackageTreeNodeViewModel> Children { get; } = new();
    public Maybe<PackageTreeNodeViewModel> Parent { get; }
    public string Name { get; }

    public double HotSpotRating => 
      _hotSpotRating + Children.Sum(c => c.HotSpotRating);

    public static PackageTreeNodeViewModel From(IPackageHistoryNode packageTree)
    {
      var packageNodeViewModelVisitor = new PackageNodeViewModelVisitor();
      packageTree.Accept(packageNodeViewModelVisitor);
      return packageNodeViewModelVisitor.ToPackageNodeViewModel();
    }
  }
}