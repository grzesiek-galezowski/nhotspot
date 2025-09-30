# .NET 9 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 9 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9 upgrade.
3. Upgrade ApplicationLogic\ApplicationLogic.csproj to .NET 9
4. Upgrade GitAnalysis\GitAnalysis.csproj to .NET 9
5. Upgrade ResultRendering\ResultRendering.csproj to .NET 9
6. Upgrade NHotSpot.Console\NHotSpot.Console.csproj to .NET 9
7. Upgrade NHotSpot.ConsoleSpecification\NHotSpot.ConsoleSpecification.csproj to .NET 9
8. Upgrade Build\Build.csproj to .NET 9
9. Upgrade ApplicationLogicSpecification\ApplicationLogicSpecification.csproj to .NET 9
10. Run unit tests to validate upgrade in the projects listed below:
   - ApplicationLogicSpecification\ApplicationLogicSpecification.csproj
   - NHotSpot.ConsoleSpecification\NHotSpot.ConsoleSpecification.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|
| LibGit2Sharp                        |   0.26.2        |  0.31.0     | Security vulnerability              |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### ApplicationLogic\ApplicationLogic.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

#### GitAnalysis\GitAnalysis.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

NuGet packages changes:
  - LibGit2Sharp should be updated from `0.26.2` to `0.31.0` (*security vulnerability*)

#### ResultRendering\ResultRendering.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

#### NHotSpot.Console\NHotSpot.Console.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

Other changes:
  - FluentCommandLineParser package compatibility may need attention for .NET 9

#### NHotSpot.ConsoleSpecification\NHotSpot.ConsoleSpecification.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

#### Build\Build.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`

#### ApplicationLogicSpecification\ApplicationLogicSpecification.csproj modifications

Project properties changes:
  - Target framework should be changed from `net7.0` to `net9.0`