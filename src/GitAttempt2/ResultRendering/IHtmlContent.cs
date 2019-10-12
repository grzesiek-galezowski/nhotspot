namespace ResultRendering
{
  public interface IHtmlContent
  {
    string ToString();
    string Render(int nesting);
  }
}