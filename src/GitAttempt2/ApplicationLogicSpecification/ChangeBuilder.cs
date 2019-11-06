using System;
using ApplicationLogic;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{
  public class ChangeBuilder
  {

    public Change Build()
    {
      string fileText = FileText;
      return ChangeFactory.CreateChange(Path, fileText, AuthorName, ChangeDate, ChangeComment, Id);
    }

    public string AuthorName { get; } = Any.Instance<string>();

    public string Id { private get; set; } = Any.Instance<string>();

    public string Path { private get; set; } = Any.Instance<string>();

    public string ChangeComment { set; private get; } = Any.String();

    public DateTimeOffset ChangeDate { set; private get; } = Any.Instance<DateTimeOffset>();

    public string FileText { set; private get; } = Any.String();
  }
}