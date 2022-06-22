using System.Collections.Generic;

namespace NHotSpot.ApplicationLogic;

public interface IPackageHistoryNode
{
    bool HasParent();
    void Accept(INodeVisitor visitor);
    void AddChild(IPackageHistoryNode newNode);
    void SetParent(IPackageHistoryNode packageHistoryNode);
}

public class PackageHistoryNode : IPackageHistoryNode
{
    private readonly IFlatPackageHistory _value;
    private readonly IEnumerable<FileHistoryNode> _files;
    private readonly List<IPackageHistoryNode> _childPackages = new List<IPackageHistoryNode>();
    private IPackageHistoryNode? _parent;

    public PackageHistoryNode(IFlatPackageHistory value, IEnumerable<FileHistoryNode> files)
    {
        _value = value;
        _files = files;
    }

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