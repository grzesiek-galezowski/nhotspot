using System.Collections.Generic;

namespace ApplicationLogic
{
  public interface IFlatPackageChangeLog : IChangeLog
  {
    void Add(IFileChangeLog fileChangeLog);
    IReadOnlyList<IFileChangeLog> Files { get; }
  }
}