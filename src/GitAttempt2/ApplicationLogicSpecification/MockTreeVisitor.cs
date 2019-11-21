using System;
using ApplicationLogic;
using AtmaFileSystem;

namespace ApplicationLogicSpecification
{
  public class MockTreeVisitor : IMockTreeVisitor
  {
    private readonly ITreeVisitor _visitor;
    private int _commits = 0;

    public MockTreeVisitor(ITreeVisitor visitor)
    {
      _visitor = visitor;
    }

    public void Modify(Change change)
    {
      _visitor.OnModified(change);
    }

    public void Rename(RelativeFilePath oldPath, Change change)
    {
      _visitor.OnRenamed(oldPath, change);
    }

    public void Copy(Change change)
    {
      _visitor.OnCopied(change);
    }

    public void Add(Change change)
    {
      _visitor.OnAdded(change);
    }

    public void Remove(RelativeFilePath removedEntryPath)
    {
      _visitor.OnRemoved(removedEntryPath);
    }

    public void Commit()
    {
      _commits++;
    }

    public int CommitCount()
    {
      return _commits;
    }

    public DirProxy Dir(string dirName)
    {
      return new DirProxy(RelativeDirectoryPath.Value(dirName), this);
    }

    public DirProxy Dir(string dirName, Action<DirProxy> dirProxyAction)
    {
      var dirProxy = Dir(dirName);
      dirProxyAction(dirProxy);
      return dirProxy;
    }
  }
}