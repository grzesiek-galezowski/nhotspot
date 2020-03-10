namespace NHotSpot.ApplicationLogic
{
  public class FileHistoryNode
  {
    private readonly IFileHistory _fileHistory;

    public FileHistoryNode(IFileHistory fileHistory)
    {
      _fileHistory = fileHistory;
    }

    public void Accept(INodeVisitor visitor)
    {
      visitor.Visit(_fileHistory);
    }
  }
}