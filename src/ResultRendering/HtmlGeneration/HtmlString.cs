namespace NHotSpot.ResultRendering.HtmlGeneration;

public class HtmlString : IHtmlContent
{
  public override string ToString()
  {
    return _tagContent;
  }

  public string Render(int nesting)
  {
    return _format.BeforeText(nesting) + _tagContent;
  }

  private readonly string _tagContent;
  private readonly IRenderingFormat _format;

  public HtmlString(string tagContent, IRenderingFormat format)
  {
    _tagContent = tagContent;
    _format = format;
  }
}