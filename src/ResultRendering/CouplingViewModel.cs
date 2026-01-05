using System.IO;
using NHotSpot.ApplicationLogic;
using Core.NullableReferenceTypesExtensions;

namespace NHotSpot.ResultRendering;

public class CouplingViewModel(
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
  public string Left { get; } = left.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
  public string Right { get; } = right.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
  public int CouplingCount { get; } = couplingCount;
  public int PercentageOfLeftCommits { get; } = percentageOfLeftCommits;
  public int PercentageOfRightCommits { get; } = percentageOfRightCommits;
  public int PercentageOfTotalCommits { get; } = percentageOfTotalCommits;
  public string LongestCommonPrefix { get; } = longestCommonPrefix.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
  public string LeftRest { get; } = leftRest.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
  public string RightRest { get; } = rightRest.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

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
