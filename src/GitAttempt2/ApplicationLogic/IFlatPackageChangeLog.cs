using System.Collections.Generic;

namespace ApplicationLogic
{
  public interface IFlatPackageChangeLog : IChangeLog
  {
    void Add(IFileChangeLog fileChangeLog);
    IEnumerable<IFileChangeLog> Files { get; }
    string Name();
  }
}