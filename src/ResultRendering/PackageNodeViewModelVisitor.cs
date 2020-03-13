using NHotSpot.ApplicationLogic;
using NullableReferenceTypesExtensions;

namespace NHotSpot.ResultRendering
{
  public class PackageNodeViewModelVisitor : INodeVisitor
  {
    private PackageTreeNodeViewModel? _currentTreeNode;
    private PackageTreeNodeViewModel? _root;

    public PackageTreeNodeViewModel ToPackageNodeViewModel()
    {
      return _root.OrThrow();
    }

    public void BeginVisiting(IFlatPackageHistory value)
    {
      var packageNodeViewModel = new PackageTreeNodeViewModel(
          value.HotSpotRating(), 
          value.PathOfCurrentVersion().ToString());
      if (_currentTreeNode != null)
      {
        _currentTreeNode.Children.Add(packageNodeViewModel);
      }
      else
      {
        _root = packageNodeViewModel;
      }

      packageNodeViewModel.Parent = _currentTreeNode;
      _currentTreeNode = packageNodeViewModel;
    }

    public void EndVisiting(IFlatPackageHistory value)
    {
      _currentTreeNode = _currentTreeNode.OrThrow().Parent;
    }

    public void Visit(IFileHistory fileHistory)
    {
      _currentTreeNode.OrThrow().Children.Add(new PackageTreeNodeViewModel(fileHistory.HotSpotRating(), fileHistory.PathOfCurrentVersion().ToString()));
    }
  }
}