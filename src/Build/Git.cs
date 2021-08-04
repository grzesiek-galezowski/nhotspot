using SimpleExec;

namespace Build
{
  internal static class Git
  {
    public static string CurrentRepositoryPath()
    {
      return Command.Read("git", " rev-parse --show-toplevel").Replace("\n", "");
    }
  }
}