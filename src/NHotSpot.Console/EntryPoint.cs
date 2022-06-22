using System.Diagnostics;

namespace NHotSpot.Console;

public static class  EntryPoint
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

        Program.Run(args);
        sw.Stop();
        System.Console.WriteLine("Total " + sw.Elapsed);
    }
}