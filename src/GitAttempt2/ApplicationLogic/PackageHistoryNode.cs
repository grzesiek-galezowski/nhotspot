using System;
using System.Collections.Generic;

namespace ApplicationLogic
{
  public class PackageHistoryNode
  {
    private readonly IFlatPackageHistory _value;
    private readonly IEnumerable<FileHistoryNode> _files;
    private readonly List<PackageHistoryNode> _childPackages = new List<PackageHistoryNode>();
    private PackageHistoryNode _parent;

    public PackageHistoryNode(IFlatPackageHistory value, IEnumerable<FileHistoryNode> files)
    {
      _value = value;
      _files = files;
    }

    public void AddChild(PackageHistoryNode newNode)
    {
      _childPackages.Add(newNode);
      newNode.SetParent(this);
    }

    private void SetParent(PackageHistoryNode packageHistoryNode)
    {
      _parent = packageHistoryNode;
    }

    public bool HasParent()
    {
      return !(_parent is null);
    }

    public void Accept(INodeVisitor visitor)
    {
      visitor.BeginVisiting(_value);
      foreach (var childFile in _files)
      {
        childFile.Accept(visitor);
      }
      foreach (var childPackage in _childPackages)
      {
        childPackage.Accept(visitor);
      }
      visitor.EndVisiting(_value);
    }
  }
}