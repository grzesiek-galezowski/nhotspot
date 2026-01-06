using AtmaFileSystem;
using NHotSpot.ApplicationLogic;
using static System.Environment;
using static System.Linq.Enumerable;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation;

public class FileProxy(RelativeFilePath fileName, RepositoryEvolution context, CommitContext commitContext)
{
  private int _complexity = 0;
  private string _author = Any.Instance<string>();

  public void Added()
  {
    context.Add(Change());
  }

  public void Modified()
  {
    context.Modify(Change());
  }

  private Change Change()
  {
    return new ChangeBuilder
    {
      Path = fileName.ToString(),
      AuthorName = _author,
      FileText = string.Join(NewLine, Repeat(" a", _complexity)),
      ChangeDate = commitContext.Date,
      Id = commitContext.ChangeId
    }.Build();
  }


  public FileProxy Complexity(int complexity)
  {
    _complexity = complexity;
    return this;
  }

  public FileProxy By(string name)
  {
    _author = name;
    return this;
  }

}
