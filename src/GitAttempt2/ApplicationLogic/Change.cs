using System;

namespace ApplicationLogic
{
    public sealed class Change
    {
        public string Path { get; }
        public DateTimeOffset ChangeDate { get; }
        public string Text { get; }
        public double Complexity { get; }
        public string Id { get; }

        public Change(
          string path, 
          string text, 
          double complexity,
          DateTimeOffset changeDate, 
          string comment, 
          string id)
        {
            Path = path;
            ChangeDate = changeDate;
            Comment = comment;
            Id = id;
            Text = text;
            Complexity = complexity;
        }

        public string Comment { get; }
    }
}
    