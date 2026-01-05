namespace NHotSpot.ApplicationLogic;

public class FileHistoryNode(IFileHistory fileHistory)
{
  public void Accept(INodeVisitor visitor)
  {
    visitor.Visit(fileHistory);
  }
}
