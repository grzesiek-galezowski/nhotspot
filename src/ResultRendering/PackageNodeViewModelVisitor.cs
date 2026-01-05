using Core.Maybe;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering;

public class PackageNodeViewModelVisitor : INodeVisitor
{
  private Maybe<PackageTreeNodeViewModel> _currentTreeNode;
  private Maybe<PackageTreeNodeViewModel> _root;

  public PackageTreeNodeViewModel ToPackageNodeViewModel()
  {
    return _root.Value();
  }

  public void BeginVisiting(IFlatPackageHistory value)
  {
    var packageNodeViewModel = new PackageTreeNodeViewModel(
        value.HotSpotRating(),
        value.PathOfCurrentVersion().ToString(),
        _currentTreeNode);

    if (_currentTreeNode.HasValue)
    {
      _currentTreeNode.Value().Children.Add(packageNodeViewModel);
    }
    else
    {
      _root = packageNodeViewModel.Just();
    }

    _currentTreeNode = packageNodeViewModel.Just();
  }

  public void EndVisiting(IFlatPackageHistory value)
  {
    _currentTreeNode = _currentTreeNode.Select(n => n.Parent);
  }

  public void Visit(IFileHistory fileHistory)
  {
    _currentTreeNode.Value()
        .Children.Add(
            new PackageTreeNodeViewModel(
                fileHistory.HotSpotRating(),
                fileHistory.PathOfCurrentVersion().ToString(),
                Maybe<PackageTreeNodeViewModel>.Nothing));
  }
}