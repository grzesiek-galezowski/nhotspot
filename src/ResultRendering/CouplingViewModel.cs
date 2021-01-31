using System.IO;
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
    public string LongestCommonPrefix { get; }
    public string LeftRest { get; }
    public string RightRest { get; }

    public CouplingViewModel(
      string left,
      string right,
      int couplingCount,
      int percentageOfLeftCommits,
      int percentageOfRightCommits,
      int percentageOfTotalCommits, 
      string longestCommonPrefix, 
      string leftRest, 
      string rightRest)
    {
      Left = left.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      Right = right.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      CouplingCount = couplingCount;
      PercentageOfLeftCommits = percentageOfLeftCommits;
      PercentageOfRightCommits = percentageOfRightCommits;
      PercentageOfTotalCommits = percentageOfTotalCommits;
      LongestCommonPrefix = longestCommonPrefix.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      LeftRest = leftRest.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      RightRest = rightRest.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    public static CouplingViewModel From<TPath>(ICoupling<TPath> c) where TPath : notnull
    {
      return new CouplingViewModel(
        c.Left.ToString(),
        c.Right.ToString(),
        c.CouplingCount,
        c.PercentageOfLeftCommits, 
        c.PercentageOfRightCommits, 
        c.PercentageOfTotalCommits,
        c.LongestCommonPathPrefix,
        c.Left.ToString().Remove(0, c.LongestCommonPathPrefix.Length),
        c.Right.ToString().Remove(0, c.LongestCommonPathPrefix.Length)
      );
    }
  }
}