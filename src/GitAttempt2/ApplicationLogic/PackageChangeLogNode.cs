using System;
using System.Collections.Generic;

namespace ApplicationLogic
{
  public class PackageChangeLogNode
  {
    private readonly IFlatPackageChangeLog _value;
    private readonly IEnumerable<FileChangeLogNode> _files;
    private readonly List<PackageChangeLogNode> _childPackages = new List<PackageChangeLogNode>();
    private PackageChangeLogNode _parent;

    public PackageChangeLogNode(IFlatPackageChangeLog value, IEnumerable<FileChangeLogNode> files)
    {
      _value = value;
      _files = files;
    }

    public void AddChild(PackageChangeLogNode newNode)
    {
      _childPackages.Add(newNode);
      newNode.SetParent(this);
    }

    private void SetParent(PackageChangeLogNode packageChangeLogNode)
    {
      _parent = packageChangeLogNode;
    }

    public bool HasParent()
    {
      return _parent is object;
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
      visitor.EndVisiting();
    }
  }
}