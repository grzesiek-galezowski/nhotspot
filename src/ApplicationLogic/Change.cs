using System;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic
{
    public sealed class Change
    {
        public RelativeFilePath Path { get; }
        public DateTimeOffset ChangeDate { get; }
        public string AuthorName { get; }
        public Lazy<double> Complexity { get; }
        public string Id { get; }

        public Change(
          RelativeFilePath path,
          Lazy<double> complexity,
          DateTimeOffset changeDate,
          string authorName,
          string id)
        {
            Path = path;
            ChangeDate = changeDate;
            AuthorName = authorName;
            Id = id;
            Complexity = complexity;
        }
    }
}    