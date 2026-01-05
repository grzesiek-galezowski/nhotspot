namespace NHotSpot.ResultRendering.HtmlGeneration;

public class HtmlAttribute(string name, string content)
{
  public string Render()
  {
    return $"{name}=\"{content}\"";
  }

  public override string ToString()
  {
    return Render();
  }
}