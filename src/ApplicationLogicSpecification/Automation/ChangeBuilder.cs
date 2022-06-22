using System;
using NHotSpot.ApplicationLogic;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation;

public class ChangeBuilder
{

    public Change Build()
    {
        return ChangeFactory.CreateChange(
            Path, 
            new Lazy<string>(() =>FileText), 
            AuthorName,
            ChangeDate,
            Id);
    }

    public string AuthorName { get; set; } = Any.Instance<string>();

    public string Id { private get; set; } = Any.Instance<string>();

    public string Path { private get; set; } = Any.Instance<string>();

    public string ChangeComment { set; private get; } = Any.String();

    public DateTimeOffset ChangeDate { set; private get; } = Any.Instance<DateTimeOffset>();

    public string FileText { set; private get; } = Any.String();
}