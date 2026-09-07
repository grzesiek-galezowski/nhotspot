using System;
using System.Linq;
using AtmaFileSystem;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Core.Maybe;
using NHotSpot.GitAnalysis;
using Microsoft.VSDiagnostics;

namespace NHotSpot.Benchmarks;
[DryJob]
[CPUUsageDiagnoser]
public class AspNetCoreCouplingBenchmark
{
    private const string RepositoryPath = @"C:\Users\GG7477\.copilot\repos\aspnetcore";
    [Benchmark]
    public (int FileCouplings, int PackageCouplings) CalculateCoupling()
    {
        using var analysis = new GitRepoAnalysis(RepositoryPath);
        var result = analysis.Analyze(Maybe<RelativeDirectoryPath>.Nothing, "main", minChangeCount: 1, startDate: new DateTime(2014, 1, 1));
        return (result.FileCouplingMetrics().Count(), result.PackageCouplingMetrics().Count());
    }
}