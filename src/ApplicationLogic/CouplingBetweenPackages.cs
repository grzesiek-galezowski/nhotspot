using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public class CouplingBetweenPackages //bug
    : ICoupling<RelativeDirectoryPath>
{
    public RelativeDirectoryPath Left { get; }
    public RelativeDirectoryPath Right { get; }
    public int CouplingCount { get; }
    public int PercentageOfLeftCommits { get; }
    public int PercentageOfRightCommits { get; }
    public int PercentageOfTotalCommits { get; }
    public string LongestCommonPathPrefix { get; }

    public CouplingBetweenPackages(
        RelativeDirectoryPath left,
        RelativeDirectoryPath right,
        int couplingCount,
        CouplingPercentages couplingPercentages)
    {
        Left = left;
        Right = right;
        CouplingCount = couplingCount;
        PercentageOfLeftCommits = couplingPercentages.PercentageOfLeftCommits;
        PercentageOfRightCommits = couplingPercentages.PercentageOfRightCommits;
        PercentageOfTotalCommits = couplingPercentages.PercentageOfTotalCommits;
        LongestCommonPathPrefix = left.FindCommonDirectoryPathWith(right).Select(s => s.ToString()).OrElse(string.Empty);
    }


    public CouplingBetweenPackages WithSwitchedSides()
    {
        return new CouplingBetweenPackages(
            Right,
            Left,
            CouplingCount,
            new CouplingPercentages(
                PercentageOfRightCommits,
                PercentageOfLeftCommits,
                PercentageOfTotalCommits));
    }
}