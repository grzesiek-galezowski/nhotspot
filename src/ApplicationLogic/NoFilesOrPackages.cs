namespace NHotSpot.ApplicationLogic;

public class NoFilesOrPackages : IPackageHistoryNode
{
    public bool HasParent()
    {
        return false;
    }

    public void Accept(INodeVisitor visitor)
    {
      
    }

    public void AddChild(IPackageHistoryNode newNode)
    {
      
    }

    public void SetParent(IPackageHistoryNode packageHistoryNode)
    {
      
    }
}