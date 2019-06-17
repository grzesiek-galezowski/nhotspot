using System;

namespace ResultRendering
{
  public class ChangeViewModel
  {
    public DateTimeOffset ChangeDate { get; }
    public string Comment { get; }

    public ChangeViewModel(DateTimeOffset changeDate, string comment)
    {
      ChangeDate = changeDate;
      Comment = comment;
    }
  }
}