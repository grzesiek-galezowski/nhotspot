using System.Linq;

namespace ResultRendering
{
  public static class Html
  {
    public static IHtmlContent Text<T>(T obj)
    {
      return new HtmlString(obj.ToString());
    }

    public static IHtmlContent Tag(string tagName, params IHtmlContent[] children)
    {
      return new HtmlTag(tagName, children);
    }

    public static IHtmlContent Tag(string tagName, HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return new HtmlTag(tagName, attributes, children);
    }

    public static IHtmlContent Td(params IHtmlContent[] children)
    {
      return Td(new HtmlAttribute[]{ }, children);
    }
    
    public static IHtmlContent Tr(params IHtmlContent[] children)
    {
      return Tr(new HtmlAttribute[] { }, children);
    }
    
    public static IHtmlContent Td(HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return Tag("td", attributes, children);
    }
    
    public static IHtmlContent Tr(HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return Tag("tr", attributes, children);
    }

    public static HtmlAttribute[] Attribute(string name, string value)
    {
      return new[] { new HtmlAttribute(name, value)};
    }

    public static HtmlAttribute[] Attributes(params (string name, string value)[] args)
    {
      return args.Select(arg => new HtmlAttribute(arg.name, arg.value)).ToArray();
    }
  }
}