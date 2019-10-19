﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultRendering
{
  public class HtmlTag : IHtmlContent
  {
    public static HtmlTag PrettyPrinted(string tagName, IEnumerable<HtmlAttribute> attributes, IEnumerable<IHtmlContent> children)
    {
      return new HtmlTag(tagName, attributes, children, new PrettyFormat());
    }

    public static HtmlTag PrettyPrinted(string tagName, IEnumerable<IHtmlContent> children)
    {
      return PrettyPrinted(tagName, new HtmlAttribute[] { }, children);
    }

    public static HtmlTag VerbatimPrinted(string tagName, IHtmlContent child)
    {
      return new HtmlTag(tagName, new HtmlAttribute[] { }, new[] { child }, new VerbatimFormat());
    }

    public override string ToString()
    {
      return Render(0);
    }

    public string Render(int nesting)
    {
      return _renderingFormat.BeforeTagOpen(nesting) + $"<{_tagName} {RenderAttributes()}>" + _renderingFormat.AfterTagOpen() +
             RenderChildren(nesting)
             + _renderingFormat.BeforeTagClose(nesting) + $"</{_tagName}>";
    }

    private string RenderAttributes()
    {
      return String.Join(" ", _attributes.Select(a => a.Render()));
    }

    private string RenderChildren(int nesting)
    {
      return String.Join(" ", _children.Select(i => i.Render(nesting + 1) + _renderingFormat.AfterChild()));
    }

    private readonly string _tagName;
    private readonly IEnumerable<HtmlAttribute> _attributes;
    private readonly IEnumerable<IHtmlContent> _children;
    private readonly IRenderingFormat _renderingFormat;

    private HtmlTag(
      string tagName, 
      IEnumerable<HtmlAttribute> attributes, 
      IEnumerable<IHtmlContent> children, 
      IRenderingFormat renderingFormat)
    {
      _tagName = tagName;
      _attributes = attributes;
      _children = children;
      _renderingFormat = renderingFormat;
    }

    public static IHtmlContent VerbatimPrinted(string tagName, IHtmlContent[] children)
    {
      return new HtmlTag(
        tagName, 
        new HtmlAttribute[] { }, 
        children, 
        new VerbatimFormat());
    }

    public static IHtmlContent VerbatimPrinted(string tagName, HtmlAttribute[] attributes, params IHtmlContent[] children)
    {
      return new HtmlTag(
        tagName, 
        attributes, 
        children, 
        new VerbatimFormat());
    }

    public static HtmlTag AsyncTag(IEnumerable<IHtmlContent> children)
    {
      return new HtmlTag("body", new HtmlAttribute[] { }, children, new PrettyFormat());
    }
  }
}