namespace NHotSpot.ApplicationLogic
{
  public interface ICoupling<TPath>
  {
    int CouplingCount { get; }
    TPath Left { get; }
    TPath Right { get; }
    int PercentageOfLeftCommits { get; }
    int PercentageOfRightCommits { get; }
    int PercentageOfTotalCommits { get; }
    string LongestCommonPathPrefix { get; }
  }
}