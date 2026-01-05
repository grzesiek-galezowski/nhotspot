using System;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic;

public sealed class Change(
  RelativeFilePath path,
  Lazy<double> complexity,
  DateTimeOffset changeDate,
  string authorName,
  string id)
{
  public RelativeFilePath Path { get; } = path;
  public DateTimeOffset ChangeDate { get; } = changeDate;
  public string AuthorName { get; } = authorName;
  public Lazy<double> Complexity { get; } = complexity;
  public string Id { get; } = id;
}
