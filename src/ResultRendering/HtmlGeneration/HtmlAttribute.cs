namespace NHotSpot.ResultRendering.HtmlGeneration;

public class HtmlAttribute
{
  private readonly string _name;
  private readonly string _content;

  public HtmlAttribute(string name, string content)
  {
    _name = name;
    _content = content;
  }

  public string Render()
  {
    return $"{_name}=\"{_content}\"";
  }

  public override string ToString()
  {
    return Render();
  }
}