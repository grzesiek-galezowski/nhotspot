using System;

namespace NHotSpot.ApplicationLogic;

public interface ICollectCommittInfoVisitor
{
  void AddMetadata(string authorName, DateTimeOffset date);
}
