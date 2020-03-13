using NHotSpot.ApplicationLogic;

namespace NHotSpot.ResultRendering
{
  public class CouplingViewModel
  {
    public string Left { get; }
    public string Right { get; }
    public int CouplingCount { get; }
    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }
    public int PercentageOfTotalCommits { get; }

    public CouplingViewModel(
      string left,
      string right,
      int couplingCount,
      int percentageOfLeftCommits,
      int percentageOfRightCommits,
      int percentageOfTotalCommits)
    {
      Left = left;
      Right = right;
      CouplingCount = couplingCount;
      PercentageOfLeftCommits = percentageOfLeftCommits;
      PercentageOfRightCommits = percentageOfRightCommits;
      PercentageOfTotalCommits = percentageOfTotalCommits;
    }

    public static CouplingViewModel From<TPath>(ICoupling<TPath> c) where TPath : notnull
    {
      return new CouplingViewModel(
        c.Left.ToString(), 
        c.Right.ToString(), 
        c.CouplingCount, 
        c.PercentageOfLeftCommits, 
        c.PercentageOfRightCommits, 
        c.PercentageOfTotalCommits);
    }
  }
}