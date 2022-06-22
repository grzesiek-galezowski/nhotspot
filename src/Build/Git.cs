using System.Threading.Tasks;
using SimpleExec;

namespace Build;

internal static class Git
{
    public static async Task<string> CurrentRepositoryPath()
    {
        return (await Command.ReadAsync("git", " rev-parse --show-toplevel")).StandardOutput.Replace("\n", "");
    }
}