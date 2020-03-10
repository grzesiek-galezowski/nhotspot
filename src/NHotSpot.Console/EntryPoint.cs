using System.Diagnostics;

namespace NHotSpot.Console
{
  public class EntryPoint
  {
    //TODO histogram of age (how many files live each number of months)
    //TODO count complexity increase/decrease numbers and increase ratio (how many complexity drops vs complexity increases)
    //TODO add contributors count to hot spot description
    //TODO add percentage of all commits to hot spot description
    //TODO add trend - fastest increasing complexity (not no. of changes)
    static void Main(string[] args)
    {

      var sw = new Stopwatch();
      sw.Start();

      Program.Run(new []
      {
        "-r", 
        //@"C:\Users\grzes\Documents\GitHub\nscan",
        //@"c:\Users\ftw637\source\repos\vp-bots\",
        //@"C:\Users\ftw637\Documents\GitHub\nscan",
        //@"C:\Users\grzes\Documents\GitHub\kafka\",
        @"C:\Users\grzes\Documents\GitHub\nhotspot\",
        //@"C:\Users\ftw637\Documents\GitHub\nhotspot\",
        //@"c:\Users\ftw637\source\repos\vp-bot-gateway\",
        //@"C:\Users\grzes\Documents\GitHub\botbuilder-dotnet\",
        //@"C:\Users\ftw637\Documents\GitHub\botbuilder-dotnet\",
        "-b", "master",
        //"-b", "trunk",
        "--max-coupling-per-hospot", "20",
        "--max-hostpot-count", "100",
        "-o", "output.html",
        "--min-change-count", "1"
      });
      sw.Stop();
      System.Console.WriteLine("Total " + sw.Elapsed);
    }
  }
}