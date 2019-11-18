using System;
using System.Linq;
using ApplicationLogic;
using ApplicationLogicSpecification;

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