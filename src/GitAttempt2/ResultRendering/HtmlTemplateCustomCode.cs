﻿using System;
using System.Linq;
using static ResultRendering.Html;

namespace ResultRendering
{
  public partial class HtmlTemplate
  {
    public HtmlTemplate(ViewModel viewModel)
    {
      _viewModel = viewModel;
    }

  }

  public interface IHtmlContent
  {
    string ToString();
    string Render(int nesting);
  }

  public class HtmlTag : IHtmlContent
  {
    public override string ToString()
    {
      return Render(0);
    }

    public string Render(int nesting)
    {
      //bug add nesting support
      return nesting.Spaces() + $"<{_tagName} {RenderAttributes()}>" + Environment.NewLine + 
             RenderChildren(nesting) 
             + nesting.Spaces() + $"</{_tagName}>";
    }

    private string RenderAttributes()
    {
      return string.Join(" ", _attributes.Select(a => a.Render()));
    }

    private string RenderChildren(int nesting)
    {
      return string.Join(" ", _children.Select(i => i.Render(nesting + 1) + Environment.NewLine));
    }

    private readonly string _tagName;
    private readonly HtmlAttribute[] _attributes;
    private readonly IHtmlContent[] _children;

    public HtmlTag(string tagName, IHtmlContent[] children)
    : this(tagName, new HtmlAttribute[] { }, children)
    {
    }

    public HtmlTag(string tagName, HtmlAttribute[] attributes, IHtmlContent[] children)
    {
      _tagName = tagName;
      _attributes = attributes;
      _children = children;
    }
  }

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

  public static class SpacesExtensions
  {
    public static string Spaces(this int count)
    {
      return new string(Enumerable.Repeat(' ', count).ToArray());
    }
  }

  public class HtmlString : IHtmlContent
  {
    public override string ToString()
    {
      return _tagContent;
    }

    public string Render(int nesting)
    {
      return nesting.Spaces() + _tagContent;
    }

    private readonly string _tagContent;

    public HtmlString(string tagContent)
    {
      _tagContent = tagContent;
    }
  }

  public class PackageListView
  {
    public static IHtmlContent RenderFrom(PackageTreeNodeViewModel packageTree)
    {
      if (packageTree.Children.Any())
      {
        return Tag("ul", packageTree.Children.OrderByDescending(c => c.HotSpotRating)
          .Select(childPackage => Tag("li", RenderChildPackage(childPackage))).ToArray());
      }
      else
      {
        return new HtmlString(string.Empty);
      }
    }

    private static IHtmlContent RenderChildPackage(PackageTreeNodeViewModel childPackage)
    {
      if (childPackage.Children.Any())
      {
        return Tag("details", Tag("summary", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")")), RenderFrom(childPackage));
      }
      else
      {
        return Tag("span", Text(childPackage.Name + " (" + childPackage.HotSpotRating + ")"));
      }
    }
  }

}