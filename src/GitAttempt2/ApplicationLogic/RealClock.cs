using System;

namespace ApplicationLogic
{
  public class RealClock : IClock
  {
    public DateTime Now()
    {
      return DateTime.UtcNow;
    }
  }
}