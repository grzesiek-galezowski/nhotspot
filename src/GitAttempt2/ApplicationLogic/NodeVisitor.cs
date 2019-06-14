namespace ApplicationLogic
{
  public interface INodeVisitor
  {
    void BeginVisiting(IFlatPackageChangeLog value);
    void EndVisiting();
    void Visit(IFileChangeLog fileChangeLog);
  }
}