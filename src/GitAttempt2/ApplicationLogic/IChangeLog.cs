namespace ApplicationLogic
{
  public interface IChangeLog : IItemWithPath
  {
    double HotSpotRank();
    int ChangesCount();
    double ComplexityOfCurrentVersion();
    void AssignChangeCountRank(int rank);
    void AssignComplexityRank(int rank);
  }
}