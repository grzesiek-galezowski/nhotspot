using System.Collections.Generic;

namespace NHotSpot.ApplicationLogic;

public interface IPackageHistoryNode
{
  bool HasParent();
  void Accept(INodeVisitor visitor);
  void AddChild(IPackageHistoryNode newNode);
  void SetParent(IPackageHistoryNode packageHistoryNode);
}

public class PackageHistoryNode(IFlatPackageHistory value, IEnumerable<FileHistoryNode> files)
  : IPackageHistoryNode
{
  private readonly List<IPackageHistoryNode> _childPackages = new List<IPackageHistoryNode>();
  private IPackageHistoryNode? _parent;

  public void AddChild(IPackageHistoryNode newNode)
  {
    _childPackages.Add(newNode);
    newNode.SetParent(this);
  }

  public void SetParent(IPackageHistoryNode packageHistoryNode)
  {
    _parent = packageHistoryNode;
  }

  public bool HasParent()
  {
    return !(_parent is null);
  }

  public void Accept(INodeVisitor visitor)
  {
    visitor.BeginVisiting(value);
    foreach (var childFile in files)
    {
      childFile.Accept(visitor);
    }
    foreach (var childPackage in _childPackages)
    {
      childPackage.Accept(visitor);
    }
    visitor.EndVisiting(value);
  }
}
