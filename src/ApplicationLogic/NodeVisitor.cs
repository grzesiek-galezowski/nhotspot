namespace NHotSpot.ApplicationLogic;

public interface INodeVisitor
{
  void BeginVisiting(IFlatPackageHistory value);
  void EndVisiting(IFlatPackageHistory value);
  void Visit(IFileHistory fileHistory);
}