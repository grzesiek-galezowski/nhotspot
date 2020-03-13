namespace NHotSpot.ApplicationLogic
{
  public class CouplingPercentages
  {
    public CouplingPercentages(int percentageOfLeftCommits, int percentageOfRightCommits, int percentageOfTotalCommits)
    {
      PercentageOfLeftCommits = percentageOfLeftCommits;
      PercentageOfRightCommits = percentageOfRightCommits;
      PercentageOfTotalCommits = percentageOfTotalCommits;
    }

    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }
    public int PercentageOfTotalCommits { get; }

    public static CouplingPercentages CalculateUsing(
      int changesCount,
      int otherChangesCount,
      int couplingCount,
      int totalCommits)
    {
      var percentageOfLeftCommits = CalculatePercentage(couplingCount, changesCount);
      var percentageOfRightCommits = CalculatePercentage(couplingCount, otherChangesCount);
      var percentageOfTotalCommits = CalculatePercentage(couplingCount, totalCommits);
      return new CouplingPercentages(
        percentageOfLeftCommits, 
        percentageOfRightCommits, 
        percentageOfTotalCommits);
    }

    private static int CalculatePercentage(int couplingCount, int changesCount)
    {
      if(changesCount == 0)
      {
        return 0; //bug really?
      }
      return (couplingCount * 100)/changesCount;
    }
  }
}