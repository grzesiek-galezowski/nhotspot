using AtmaFileSystem;

namespace ApplicationLogic
{
  public class Coupling
  {
    public RelativeFilePath Left { get; }
    public RelativeFilePath Right { get; }
    public int CouplingCount { get; }
    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }
    public int PercentageOfTotalCommits { get; }

    public Coupling(RelativeFilePath left,
      RelativeFilePath right,
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
  }
}