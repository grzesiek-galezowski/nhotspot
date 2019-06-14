namespace ApplicationLogic
{
  public interface IChangeLog : IItemWithPath
  {
    double HotSpotRating();
    int ChangesCount();
    double ComplexityOfCurrentVersion();

  }
}