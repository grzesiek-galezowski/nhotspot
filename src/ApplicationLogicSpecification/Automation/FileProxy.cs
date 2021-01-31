using AtmaFileSystem;
using NHotSpot.ApplicationLogic;
using static System.Environment;
using static System.Linq.Enumerable;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation
{
  public class FileProxy
  {
    private readonly RelativeFilePath _fileName;
    private readonly RepositoryEvolution _context;
    private readonly CommitContext _commitContext;
    private int _complexity = 0;
    private string _author = Any.Instance<string>();

    public FileProxy(RelativeFilePath fileName, RepositoryEvolution context, CommitContext commitContext)
    {
      _fileName = fileName;
      _context = context;
      _commitContext = commitContext;
    }

    public void Added()
    {
      _context.Add(Change());
    }

    public void Modified()
    {
      _context.Modify(Change());
    }

    private Change Change()
    {
      return new ChangeBuilder
      {
        Path = _fileName.ToString(),
        AuthorName = _author,
        FileText = string.Join(NewLine, Repeat(" a", _complexity)),
        ChangeDate = _commitContext.Date,
        Id = _commitContext.ChangeId
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
}