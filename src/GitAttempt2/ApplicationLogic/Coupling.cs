namespace ApplicationLogic
{
  public class Coupling
  {
    public string Left { get; }
    public string Right { get; }
    public int CouplingCount { get; }
    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }

    public Coupling(
      string left, 
      string right, 
      int couplingCount, 
      int percentageOfLeftCommits, 
      int percentageOfRightCommits)
    {
      Left = left;
      Right = right;
      CouplingCount = couplingCount;
      PercentageOfLeftCommits = percentageOfLeftCommits;
      PercentageOfRightCommits = percentageOfRightCommits;
    }
  }
}