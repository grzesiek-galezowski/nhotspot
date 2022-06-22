using System;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public class AnalysisConfig
{
    public int MaxHotSpotCount { get; set; }
    public int MaxCouplingsPerHotSpot { get; set; }
    public string? OutputFile { get; set; }
    public string? RepoPath { get; set; }
    public string? Branch { get; set; }
    public int MinChangeCount { get; set; }
    public DateTime StartDate { get; set; }
    public Maybe<RelativeDirectoryPath> Subfolder { get; set; }
}