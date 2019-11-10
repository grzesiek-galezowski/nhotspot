using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Functional.Maybe;

namespace ApplicationLogic
{
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
      return (!currentIndentationLength.HasValue || lineIndentation < currentIndentationLength.Value);
    }

    private static bool ThereIsAny(int lineIndentation)
    {
      return lineIndentation > 0;
    }

    private static double TotalIndentations(int totalWhitespaces, Maybe<int> indentationLength)
    {
      return indentationLength.Select(il => (1d * totalWhitespaces) / il).OrElse(0);
    }

    private static int IndentationOf(string line)
    {
      return line.TakeWhile(Char.IsWhiteSpace).Count();
    }

    public static double CalculateHotSpotRating(int complexityRank, int changeCountRank)
    {
      return (2 * changeCountRank + complexityRank) / 2d;
    }

    public static IEnumerable<Coupling> CalculateCoupling(IEnumerable<IFileHistory> fileHistories, int totalCommits)
    {
      var couplingMetric = new List<Coupling>();
      Console.WriteLine("Calculating coupling");
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var i = 0;
      foreach(var history in fileHistories)
      {
        i++;
        foreach (var otherHistory in fileHistories.Skip(i))
        {
          var coupling = history.CalculateCouplingTo(otherHistory, totalCommits);
          if (coupling.CouplingCount != 0)
          {
            couplingMetric.Add(coupling);
          }
        }
      }
      stopwatch.Stop();
      Console.WriteLine("Calculating coupling finished " + stopwatch.ElapsedMilliseconds);
      return couplingMetric
        .Where(c => c.CouplingCount > ArbitraryLimit)
        .OrderByDescending(c => c.CouplingCount);
    }
    
  }
}