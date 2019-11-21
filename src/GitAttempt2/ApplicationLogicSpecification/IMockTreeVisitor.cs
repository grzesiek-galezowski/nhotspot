using System;
using ApplicationLogic;
using AtmaFileSystem;

namespace ApplicationLogicSpecification
{
  public interface IMockTreeVisitor
  {
    void Modify(Change change);
    void Rename(RelativeFilePath oldPath, Change change);
    void Copy(Change change);
    void Add(Change change);
    void Remove(RelativeFilePath removedEntryPath);
    void Commit();
    int CommitCount();
    DirProxy Dir(string dirName);
    DirProxy Dir(string dirName, Action<DirProxy> dirProxyAction);
  }
}