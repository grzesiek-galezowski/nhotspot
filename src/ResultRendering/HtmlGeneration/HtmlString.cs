namespace NHotSpot.ResultRendering.HtmlGeneration;

public class HtmlString(string tagContent, IRenderingFormat format) : IHtmlContent
{
  public override string ToString()
  {
    return tagContent;
  }

  public string Render(int nesting)
  {
    return format.BeforeText(nesting) + tagContent;
  }
}