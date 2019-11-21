using System;
using System.Linq;
using AtmaFileSystem;

namespace ApplicationLogicSpecification
{
  public class FileProxy
  {
    private readonly RelativeFilePath _fileName;
    private readonly MockTreeVisitor _context;
    private int _complexity = 0;

    public FileProxy(RelativeFilePath fileName, MockTreeVisitor context)
    {
      _fileName = fileName;
      _context = context;
    }

    public void Add()
    {
      _context.Add(new ChangeBuilder()
      {
        Path = _fileName.ToString(),
        FileText = string.Join(Environment.NewLine, Enumerable.Repeat(" a", _complexity))
      }.Build());
    }

    public FileProxy Complexity(int complexity)
    {
      _complexity = complexity;
      return this;
    }
  }
}