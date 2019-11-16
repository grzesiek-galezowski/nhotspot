using System;

namespace ResultRendering
{
  public class ChangeViewModel
  {
    public DateTimeOffset ChangeDate { get; }
    public string Comment { get; }
    public string Author { get; }

    public ChangeViewModel(DateTimeOffset changeDate, string comment, string author)
    {
      ChangeDate = changeDate;
      Comment = comment;
      Author = author;
    }
  }
}