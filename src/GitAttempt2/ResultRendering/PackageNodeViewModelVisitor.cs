using System;
using ApplicationLogic;

namespace ResultRendering
{
  public class PackageNodeViewModelVisitor : INodeVisitor
  {
    private PackageTreeNodeViewModel _currentTreeNode;
    private PackageTreeNodeViewModel _root;

    public PackageTreeNodeViewModel ToPackageNodeViewModel()
    {
      return _root;
    }

    public void BeginVisiting(IFlatPackageChangeLog value)
    {
      var packageNodeViewModel = new PackageTreeNodeViewModel(
          value.HotSpotRating(), 
          value.PathOfCurrentVersion());
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

    public void EndVisiting(IFlatPackageChangeLog value)
    {
      _currentTreeNode = _currentTreeNode.Parent;
    }

    public void Visit(IFileHistory fileHistory)
    {
      _currentTreeNode.Children.Add(new PackageTreeNodeViewModel(fileHistory.HotSpotRating(), fileHistory.PathOfCurrentVersion()));
    }
  }
}