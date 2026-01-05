using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Maybe;
using NHotSpot.ApplicationLogic;
using Core.NullableReferenceTypesExtensions;

namespace NHotSpot.ResultRendering;

public class PackageTreeNodeViewModel(
  double hotSpotRating,
  string pathOfCurrentVersion,
  Maybe<PackageTreeNodeViewModel> parent)
{
  public List<PackageTreeNodeViewModel> Children { get; } = new();
  public Maybe<PackageTreeNodeViewModel> Parent { get; } = parent;
  public string Name { get; } = Path.GetFileName(pathOfCurrentVersion).OrThrow();

  public double HotSpotRating =>
      hotSpotRating + Children.Sum(c => c.HotSpotRating);

  public static PackageTreeNodeViewModel From(IPackageHistoryNode packageTree)
  {
    var packageNodeViewModelVisitor = new PackageNodeViewModelVisitor();
    packageTree.Accept(packageNodeViewModelVisitor);
    return packageNodeViewModelVisitor.ToPackageNodeViewModel();
  }
}
