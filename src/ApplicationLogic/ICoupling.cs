namespace NHotSpot.ApplicationLogic;

public interface ICoupling<out TPath> where TPath : notnull
{
    int CouplingCount { get; }
    TPath Left { get; }
    TPath Right { get; }
    int PercentageOfLeftCommits { get; }
    int PercentageOfRightCommits { get; }
    int PercentageOfTotalCommits { get; }
    string LongestCommonPathPrefix { get; }
}