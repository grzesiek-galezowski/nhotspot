using System;
using System.Linq;
using AtmaFileSystem;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation
{
  public class FileProxy
  {
    private readonly RelativeFilePath _fileName;
    private readonly RepositoryEvolution _context;
    private int _complexity = 0;
    private string _author = Any.Instance<string>();

    public FileProxy(RelativeFilePath fileName, RepositoryEvolution context)
    {
      _fileName = fileName;
      _context = context;
    }

    public void Added()
    {
      _context.Add(new ChangeBuilder()
      {
        Path = _fileName.ToString(),
        AuthorName = _author,
        FileText = string.Join(Environment.NewLine, Enumerable.Repeat(" a", _complexity))
      }.Build());
    }

    public FileProxy Complexity(int complexity)
    {
      _complexity = complexity;
      return this;
    }

    public FileProxy Author(string name)
    {
      _author = name;
      return this;
    }
  }
}