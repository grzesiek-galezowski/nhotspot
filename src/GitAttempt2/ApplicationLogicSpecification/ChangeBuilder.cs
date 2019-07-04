using System;
using ApplicationLogic;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification
{
  public class ChangeBuilder
  {

    public Change Build()
    {
      return ChangeFactory.CreateChange(Path, FileText, ChangeDate, ChangeComment);
    }

    public string Path { private get; set; } = Any.Instance<string>();

    public string ChangeComment { set; private get; } = Any.String();

    public DateTimeOffset ChangeDate { set; private get; } = Any.Instance<DateTimeOffset>();

    public string FileText { set; private get; } = Any.String();
  }
}