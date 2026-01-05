using System;

namespace NHotSpot.ApplicationLogic;

public interface IClock
{
  DateTime Now();
}