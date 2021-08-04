using System.IO;
using NHotSpot.ApplicationLogic;
using NullableReferenceTypesExtensions;

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
      var leftAsString = c.Left.ToString().OrThrow();
      var rightAsString = c.Right.ToString().OrThrow();
      return new CouplingViewModel(
        leftAsString,
        rightAsString,
        c.CouplingCount,
        c.PercentageOfLeftCommits, 
        c.PercentageOfRightCommits, 
        c.PercentageOfTotalCommits,
        c.LongestCommonPathPrefix,
        leftAsString.Remove(0, c.LongestCommonPathPrefix.Length),
        rightAsString.Remove(0, c.LongestCommonPathPrefix.Length)
      );
    }
  }
}