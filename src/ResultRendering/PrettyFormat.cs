using System;

namespace NHotSpot.ResultRendering
{
  public interface IRenderingFormat
  {
    string BeforeTagOpen(int nesting);
    string AfterTagOpen();
    string BeforeTagClose(int nesting);
    string AfterChild();
    string BeforeText(int nesting);
  }

  public class VerbatimFormat : IRenderingFormat
  {
    public string BeforeTagOpen(int nesting)
    {
      return nesting.Spaces();
    }

    public string AfterTagOpen()
    {
      return string.Empty;
    }

    public string BeforeTagClose(int nesting)
    {
      return string.Empty;
    }

    public string AfterChild()
    {
      return string.Empty;
    }

    public string BeforeText(int nesting)
    {
      return string.Empty;
    }
  }

  public class PrettyFormat : IRenderingFormat
  {
    public string BeforeTagOpen(int nesting)
    {
      return nesting.Spaces();
    }

    public string AfterTagOpen()
    {
      return Environment.NewLine;
    }

    public string BeforeTagClose(int nesting)
    {
      return nesting.Spaces();
    }

    public string AfterChild()
    {
      return Environment.NewLine;
    }

    public string BeforeText(int nesting)
    {
      return nesting.Spaces();
    }
  }
}