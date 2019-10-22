namespace ResultRendering
{
    public class HistogramViewModel
    {
        public HistogramViewModel(string description, string labels, string data)
        {
            Labels = labels;
            Description = description;
            Data = data;
        }

        public string Labels { get; }
        public string Description { get; }
        public string Data { get; }
    }
}