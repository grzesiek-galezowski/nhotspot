using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHotSpot.ResultRendering
{
  public static class Html
  {
    public static IHtmlContent Text<T>(T obj) where T : notnull
    {
      return new HtmlString(obj.ToString(), new PrettyFormat());
    }

    public static IHtmlContent Text(params object[] objects)
    {
      var strBuilder = new StringBuilder();
      foreach (var obj in objects)
      {
        strBuilder.Append(obj);
      }

      return new HtmlString(strBuilder.ToString(), new PrettyFormat());
    }

    public static IHtmlContent VerbatimText<T>(T obj) where T : notnull
    {
      return new HtmlString(obj.ToString(), new VerbatimFormat());
    }

    public static IHtmlContent Tag(string tagName, params IHtmlContent[] children)
    {
      return HtmlTag.PrettyPrinted(tagName, children);
    }

    public static IHtmlContent Body(params IHtmlContent[] children)
    {
      return Tag("body", children);
    }

    public static IHtmlContent Tag(string tagName, IEnumerable<IHtmlContent> children)
    {
      return HtmlTag.PrettyPrinted(tagName, children);
    }

    public static IHtmlContent VerbatimTag(string tagName, params IHtmlContent[] children)
    {
      return HtmlTag.VerbatimPrinted(tagName, children);
    }

    public static IHtmlContent Tag(string tagName, HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return HtmlTag.PrettyPrinted(tagName, attributes, children);
    }

    public static IHtmlContent Tag(string tagName, IEnumerable<HtmlAttribute> attributes, params IHtmlContent[] children)
    {
      return HtmlTag.PrettyPrinted(tagName, attributes, children);
    }

    public static IHtmlContent VerbatimTag(string tagName, HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return HtmlTag.VerbatimPrinted(tagName, attributes, children);
    }

    public static IHtmlContent Pre(string content)
    {
      return HtmlTag.VerbatimPrinted("pre", VerbatimText(content));
    }

    public static IHtmlContent Td(params IHtmlContent[] children)
    {
      return Td(new HtmlAttribute[]{ }, children);
    }
    
    public static IHtmlContent Tr(params IHtmlContent[] children)
    {
      return Tr(new HtmlAttribute[] { }, children);
    }

    public static IHtmlContent Th(params IHtmlContent[] children)
    {
      return Th(new HtmlAttribute[] { }, children);
    }
    
    public static IHtmlContent Td(HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return Tag("td", attributes, children);
    }
    
    public static IHtmlContent Tr(HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return Tag("tr", attributes, children);
    }

    public static IHtmlContent Th(HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return Tag("th", attributes, children);
    }

    public static HtmlAttribute[] Attribute(string name, string value)
    {
      return new[] { new HtmlAttribute(name, value)};
    }

    public static IEnumerable<HtmlAttribute> Attributes(params (string name, string value)[] args)
    {
      return args.Select(arg => new HtmlAttribute(arg.name, arg.value));
    }

    public static IHtmlContent H(int i, string text)
    {
        return H(i, new HtmlAttribute[]{ }, text);
    }

    public static IHtmlContent H(int i, HtmlAttribute[] attribute, string text)
    {
        return VerbatimTag("h"+i, attribute, VerbatimText(text));
    }

    public static IHtmlContent Strong(IHtmlContent content)
    {
      return Html.Tag("strong", content);
    }
  }
}