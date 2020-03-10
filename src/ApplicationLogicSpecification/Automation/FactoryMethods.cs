using System;
using System.Linq;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification.Automation
{
  static internal class FactoryMethods
  {
    public static Change File(string fileName, int complexity)
    {
      return new ChangeBuilder
      {
        Path = fileName,
        FileText = String.Join(Environment.NewLine, Enumerable.Repeat(" a", complexity))
      }.Build();
    }
  }
}