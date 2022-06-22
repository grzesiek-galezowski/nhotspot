using System;
using FluentAssertions;
using NHotSpot.Console;
using NUnit.Framework;

namespace NHotSpot.ConsoleSpecification;

public class IntegrationSpecification
{
    [Test]
    public void ShouldNotThrowExceptionsWhenAskedForHelp()
    {
        new Action(() => Program.Run(new[] { "--help" })).Should().NotThrow();
    }
}