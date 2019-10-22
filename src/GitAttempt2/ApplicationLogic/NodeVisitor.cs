namespace ApplicationLogic
{
  public interface INodeVisitor
  {
    void BeginVisiting(IFlatPackageChangeLog value);
    void EndVisiting(IFlatPackageChangeLog value);
    void Visit(IFileHistory fileHistory);
  }
}