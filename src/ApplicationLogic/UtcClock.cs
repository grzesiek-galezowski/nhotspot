using System;

namespace NHotSpot.ApplicationLogic;

public class UtcClock : IClock
{
  public DateTime Now()
  {
    return DateTime.UtcNow;
  }
}