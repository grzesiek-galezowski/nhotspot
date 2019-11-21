using System;

namespace ApplicationLogicSpecification
{
  public static class TimesExtensions
  {
    public static void Times(this int num, Action action)
    {
      for (int i = 0; i < num; ++i)
      {
        action();
      }
    }
  }
}