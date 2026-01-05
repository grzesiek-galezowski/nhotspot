using System;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification.Automation;

public interface IRepositoryEvolution
{
  void Add(Change change);
  void CommitChanges();
  void Commit(Action<DirProxy> action);
}