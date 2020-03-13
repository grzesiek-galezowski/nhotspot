using static NHotSpot.ResultRendering.Html;

namespace NHotSpot.ResultRendering
{
  public static class DocumentHeaderView
  {
    public static IHtmlContent Render()
    {
      return Tag("head",
        VerbatimText("<meta charset=\"UTF-8\">"),
        Tag("style", Text(StyleSheet())),
        VerbatimTag("title", VerbatimText("Line ChartText")),
        VerbatimTag("script", Attribute("src", "https://cdn.jsdelivr.net/npm/chart.js@2.8.0"))
      );
    }

    private static string StyleSheet()
    {
      return @"p {
			  padding: 0;
			  margin: 0;
		  }

		  ol {
			  -ms-columns: 2;
			  -webkit-columns: 2;
			  -moz-columns: 2;
			  columns: 2;
			  font-weight: normal
		  }

		  body {
			  font-family: Arial !important;
		  }";
    }
  }
}
