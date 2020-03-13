using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic
{
  public class CouplingBetweenFiles : ICoupling<RelativeFilePath>
  {
    public RelativeFilePath Left { get; }
    public RelativeFilePath Right { get; }
    public int CouplingCount { get; }
    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }
    public int PercentageOfTotalCommits { get; }

    public CouplingBetweenFiles(
      RelativeFilePath left,
      RelativeFilePath right,
      int couplingCount, 
      CouplingPercentages couplingPercentages)
    {
      Left = left;
      Right = right;
      CouplingCount = couplingCount;
      PercentageOfLeftCommits = couplingPercentages.PercentageOfLeftCommits;
      PercentageOfRightCommits = couplingPercentages.PercentageOfRightCommits;
      PercentageOfTotalCommits = couplingPercentages.PercentageOfTotalCommits;
    }

    public CouplingBetweenFiles WithSwitchedSides()
    {
      return new CouplingBetweenFiles(Right, Left, CouplingCount, new CouplingPercentages(PercentageOfRightCommits, PercentageOfLeftCommits, PercentageOfTotalCommits));
    }
  }
}