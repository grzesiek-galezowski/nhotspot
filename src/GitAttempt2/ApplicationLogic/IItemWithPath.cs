using AtmaFileSystem;

namespace ApplicationLogic
{
  public interface IItemWithPath<T>
  {
    T PathOfCurrentVersion();
  }
}