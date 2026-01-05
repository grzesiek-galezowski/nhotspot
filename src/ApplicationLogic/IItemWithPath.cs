namespace NHotSpot.ApplicationLogic;

public interface IItemWithPath<out T> where T : notnull
{
  T PathOfCurrentVersion();
}