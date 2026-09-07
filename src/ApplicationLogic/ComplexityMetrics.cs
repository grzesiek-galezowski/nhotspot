using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public static class ComplexityMetrics
{
  private static readonly Regex NewlineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
  private const int ArbitraryLimit = 0; //bug make that a percentage?

  public static double CalculateComplexityFor(string contentText)
  {
    return CalculateComplexityFor(NewlineRegex.Split(contentText));
  }

  private static double CalculateComplexityFor(IReadOnlyCollection<string> linesInFile)
  {
    var totalWhitespaces = 0;
    var currentIndentationLength = Maybe<int>.Nothing;
    foreach (var line in linesInFile)
    {
      var lineIndentation = IndentationOf(line);
      if (ThereIsAny(lineIndentation) && IsBetter(lineIndentation, currentIndentationLength))
      {
        currentIndentationLength = lineIndentation.ToMaybe();
      }

      totalWhitespaces += lineIndentation;
    }

    return TotalIndentations(totalWhitespaces, currentIndentationLength);
  }

  private static bool IsBetter(int lineIndentation, Maybe<int> currentIndentationLength)
  {
    return (!currentIndentationLength.HasValue || lineIndentation < currentIndentationLength.Value());
  }

  private static bool ThereIsAny(int lineIndentation)
  {
    return lineIndentation > 0;
  }

  private static double TotalIndentations(int totalWhitespaces, Maybe<int> indentationLength)
  {
    return indentationLength.Select(il => (1d * totalWhitespaces) / il).OrElse(0d);
  }

  private static int IndentationOf(string line)
  {
    return line.TakeWhile(char.IsWhiteSpace).Count();
  }

  public static double CalculateHotSpotRating(int complexityRank, int changeCountRank)
  {
    return 2 * changeCountRank + complexityRank / 2d;
  }

  public static IEnumerable<TCoupling> CalculateCoupling<TCoupling, THistory, TPath>(
      IEnumerable<THistory> histories, int totalCommits)
      where TCoupling : ICoupling<TPath>
      where THistory : ICouplingSource<TCoupling, THistory>
      where TPath : notnull
  {
    var historiesAsList = histories.ToList();
    var couplingMetric = new List<TCoupling>();
    Console.WriteLine("Calculating coupling");
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    Parallel.For(0, historiesAsList.Count,
      () => new List<TCoupling>(),
      (i, _, localCouplings) =>
      {
        var currentHistory = historiesAsList[i];
        for (var j = historiesAsList.Count - 1; j > i; j--)
        {
          var otherHistory = historiesAsList[j];
          var couplingCount = currentHistory.CalculateCouplingCountTo(otherHistory);
          if (couplingCount > ArbitraryLimit)
          {
            localCouplings.Add(currentHistory.CalculateCouplingTo(otherHistory, totalCommits, couplingCount));
          }
        }

        return localCouplings;
      },
      localCouplings =>
      {
        lock (couplingMetric)
        {
          couplingMetric.AddRange(localCouplings);
        }
      });
    stopwatch.Stop();
    Console.WriteLine("Calculating coupling finished " + stopwatch.ElapsedMilliseconds);
    return couplingMetric.OrderByDescending(c => c.CouplingCount);
  }
}
