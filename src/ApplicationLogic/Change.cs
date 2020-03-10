using System;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic
{
    public sealed class Change
    {
        public RelativeFilePath Path { get; }
        public DateTimeOffset ChangeDate { get; }
        public string AuthorName { get; }
        public string Text { get; }
        public Lazy<double> Complexity { get; }
        public string Id { get; }

        public Change(RelativeFilePath path,
          string text,
          Lazy<double> complexity,
          DateTimeOffset changeDate,
          string authorName,
          string comment,
          string id)
        {
            Path = path;
            ChangeDate = changeDate;
            AuthorName = authorName;
            Comment = comment;
            Id = id;
            Text = text;
            Complexity = complexity;
        }

        public string Comment { get; }
    }
}
    