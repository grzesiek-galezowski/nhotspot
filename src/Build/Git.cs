using SimpleExec;

namespace Build
{
  static internal class Git
  {
    public static string CurrentRepositoryPath()
    {
      return Command.Read("git", " rev-parse --show-toplevel").Replace("\n", "");
    }
  }
}