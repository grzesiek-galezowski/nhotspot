namespace ResultRendering
{
    public class ChartView
    {
        private readonly string _id;

        public ChartView(string id)
        {
            _id = id;
        }

        //bug pass arrays instead of strings
        public string ChartScript(string labelsString, string dataString, string description)
        {
            var script = @"
              var ctx = document.getElementById('"+ _id + @"').getContext('2d');
              var chart = new Chart(ctx, {
                  // The type of chart we want to create
                  type: 'line',
                  options: {
                      elements: {
                          line: {
                              tension: 0 // disables bezier curves
                          }
                      }
                  },
                  // The data for our dataset
                  data: {
                      labels: [" + labelsString + @"], //example '1', '2', '3'
                      datasets: [{
                          label: '" + description + @"',
                          fill: false,
                          borderColor: 'rgb(255, 99, 132)',
                          data: [" + dataString + @"]
                      }]
                  },
      
              });";
            return script;
        }

        public IHtmlContent ChartDiv(int height)
        {
            return Html.Tag("div", Html.Attribute("class", "container"),
                Html.Tag("canvas", Html.Attributes(("id", _id), ("height", height.ToString())))
            );
        }
    }
}