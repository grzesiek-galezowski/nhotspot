using ApplicationLogic;
using ResultRendering;



namespace GitAttempt2
{
  public class Program
  {
    //TODO coupling metrics (should not be that hard)
    //TODO histogram of age (how many files live each number of months)
    //TODO package metrics as tree instead of plain list!!!
    //TODO count complexity increase/decrease numbers and increase ratio (how many complexity drops vs complexity increases)
    //TODO add per package calculations (both flat and nested, which includes complexity from nested modules)
    //TODO add contributors count to hot spot description
    //TODO add percentage of all commits to hot spot description
    //TODO add trend - fastest increasing complexity (not no. of changes)
    static void Main(string[] args)
    {
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\nscan\", "master");
      var analysisResult = GitRepoAnalysis.Analyze(@"c:\Users\ftw637\Documents\GitHub\kafka\", "trunk");

      new ConsoleRendering().Show(analysisResult);
      new HtmlChartOutput().Show(analysisResult);
    }
  }
}