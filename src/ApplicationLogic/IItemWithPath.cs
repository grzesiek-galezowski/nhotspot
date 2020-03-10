namespace NHotSpot.ApplicationLogic
{
  public interface IItemWithPath<T>
  {
    T PathOfCurrentVersion();
  }
}