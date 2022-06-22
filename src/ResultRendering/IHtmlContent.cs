namespace NHotSpot.ResultRendering;

public interface IHtmlContent
{
    string ToString();
    string Render(int nesting);
}