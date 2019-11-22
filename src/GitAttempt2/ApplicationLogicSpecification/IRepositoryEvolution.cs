using System;
using ApplicationLogic;
using AtmaFileSystem;
using LibGit2Sharp;

namespace ApplicationLogicSpecification
{
  public interface IRepositoryEvolution
  {
    void Add(Change change);
    void CommitChanges();
    void Commit(Action<DirProxy> action);
  }
}