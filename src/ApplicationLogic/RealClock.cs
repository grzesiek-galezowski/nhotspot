using System;

namespace NHotSpot.ApplicationLogic
{
  public class RealClock : IClock
  {
    public DateTime Now()
    {
      return DateTime.UtcNow;
    }
  }
}