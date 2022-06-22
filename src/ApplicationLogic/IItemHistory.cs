namespace NHotSpot.ApplicationLogic;

public interface IItemHistory<T> : IItemWithPath<T>
{
    double HotSpotRating();
    int ChangesCount();
    double ComplexityOfCurrentVersion();
}