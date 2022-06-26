namespace NHotSpot.ResultRendering.HtmlGeneration;

public interface IHtmlContent
{
  string ToString();
  string Render(int nesting);
}