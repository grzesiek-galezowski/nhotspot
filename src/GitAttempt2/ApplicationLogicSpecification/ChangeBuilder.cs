using System;
using ApplicationLogic;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;

namespace ApplicationLogicSpecification
{
  public class ChangeBuilder
  {

    public Change Build()
    {
      Path = Root.Any.String();
      return ChangeFactory.CreateChange(Path, FileText, ChangeDate, ChangeComment);
    }

    public string Path { private get; set; }

    public string ChangeComment { set; private get; } = Root.Any.String();

    public DateTimeOffset ChangeDate { set; private get; } = Root.Any.Instance<DateTimeOffset>();

    public string FileText { set; private get; } = Root.Any.String();
  }
}