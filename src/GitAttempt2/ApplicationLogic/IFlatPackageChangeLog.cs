using System.Collections.Generic;

namespace ApplicationLogic
{
  public interface IFlatPackageChangeLog : IChangeLog
  {
    void Add(IFileHistory fileHistory);
    IEnumerable<IFileHistory> Files { get; }
    string Name();
  }
}