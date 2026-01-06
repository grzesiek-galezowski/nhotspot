using System;
using System.Linq;
using ApplicationLogicSpecification.Automation;
using FluentAssertions;
using NUnit.Framework;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AtmaFileSystem;
using Core.Maybe;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification;

/// <summary>
/// Tests for backward git history traversal fixes.
/// When processing commits backward (newest to oldest), we need to:
/// 1. Handle files that don't exist in HEAD (deleted before HEAD)
/// 2. Preserve file history even when files are "added" (in forward time)
/// 3. Handle renames correctly when going backward
/// </summary>
public class BackwardTraversalSpecification
{
  [Test]
  public void ShouldPreserveFileHistoryWhenFileWasAddedInHistoryButExistsInHead()
  {
    // Scenario: File added in an older commit, then modified, exists in HEAD
    // The file should retain its full history including all modifications
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);

    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      // Commit 1 (oldest): Add file
      flow.Commit(commit =>
      {
        commit.Date(date1);
        commit.File("A.cs").By("Alice").Complexity(5).Added();
      });

      // Commit 2 (middle): Modify file
      flow.Commit(commit =>
      {
        commit.Date(date2);
        commit.File("A.cs").By("Bob").Complexity(10).Modified();
      });

      // Commit 3 (HEAD): Modify file again
      flow.Commit(commit =>
      {
        commit.Date(date3);
        commit.File("A.cs").By("Charlie").Complexity(15).Modified();
      });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount().ToList();

    entries.Should().HaveCount(1, "the file should be in the result");

    var fileHistory = entries[0];
    fileHistory.PathOfCurrentVersion().Should().Be(RelativeFilePath("A.cs"));
    fileHistory.ChangesCount().Should().Be(3, "file was added and modified twice");
    fileHistory.ComplexityOfCurrentVersion().Should().Be(15, "complexity should be from most recent commit");

    // Verify all changes are recorded
    fileHistory.Entries.Should().HaveCount(3, "should have all three commits");
  }

  [Test]
  public void ShouldIgnoreModificationsForRemovedFiles()
  {
    // Direct unit test: When a file is marked as removed,
    // subsequent modifications should be ignored
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);

    var driver = new RepoAnalysisDriver();
    var visitor = new CollectFileChangeRateFromCommitVisitor(
        driver.Clock, 
        0, 
        Maybe<RelativeDirectoryPath>.Nothing);

    // Add file A
    var change1 = new ChangeBuilder
    {
      Path = "A.cs",
      AuthorName = "Alice",
      FileText = string.Join(Environment.NewLine, Enumerable.Repeat(" a", 5)),
      ChangeDate = date1,
      Id = "commit1"
    }.Build();
    visitor.OnAdded(change1);

    // Modify file A
    var change2 = new ChangeBuilder
    {
      Path = "A.cs",
      AuthorName = "Bob",
      FileText = string.Join(Environment.NewLine, Enumerable.Repeat(" a", 10)),
      ChangeDate = date2,
      Id = "commit2"
    }.Build();
    visitor.OnModified(change2);

    // Mark file as removed (simulating backward traversal reaching the file's add point)
    visitor.OnRemoved(RelativeFilePath("A.cs"));

    // Try to modify file A again - should be ignored since it's removed
    var change3 = new ChangeBuilder
    {
      Path = "A.cs",
      AuthorName = "Charlie",
      FileText = string.Join(Environment.NewLine, Enumerable.Repeat(" a", 15)),
      ChangeDate = date3,
      Id = "commit3"
    }.Build();
    visitor.OnModified(change3);

    // File should still be in results with its 2 changes (before removal)
    var result = visitor.Result().ToList();
    result.Should().HaveCount(1, "file should preserve its history even after being marked as removed");

    var fileHistory = result[0];
    fileHistory.PathOfCurrentVersion().Should().Be(RelativeFilePath("A.cs"));
    fileHistory.ChangesCount().Should().Be(2, "should have 2 changes: add and modify before removal, ignore modify after removal");
  }

  [Test]
  public void ShouldHandleFileRenameInBackwardTraversal()
  {
    // Scenario: File was renamed from OldName.cs to NewName.cs
    // Forward: OldName.cs -> (rename) -> NewName.cs
    // Backward: HEAD has NewName.cs -> going back we "unrename" to OldName.cs
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);

    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      // Commit 1 (oldest): Add file with original name
      flow.Commit(commit =>
      {
        commit.Date(date1);
        commit.File("OldName.cs").By("Alice").Complexity(5).Added();
      });

      // Commit 2 (middle): Rename file
      flow.Commit(commit =>
      {
        commit.Date(date2);
        commit.File("OldName.cs").By("Bob").Complexity(7)
          .RenamedTo("NewName.cs");
      });

      // Commit 3 (HEAD): Modify renamed file
      flow.Commit(commit =>
      {
        commit.Date(date3);
        commit.File("NewName.cs").By("Charlie").Complexity(10).Modified();
      });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount().ToList();
    
    entries.Should().HaveCount(1, "renamed file should be tracked as single file");
    var fileHistory = entries[0];
    
    // The file should have the NEW name (from HEAD)
    fileHistory.PathOfCurrentVersion().Should().Be(RelativeFilePath("NewName.cs"));
    
    // Should have all changes: add, rename, modify
    fileHistory.ChangesCount().Should().Be(3);
    fileHistory.ComplexityOfCurrentVersion().Should().Be(10);
  }

  [Test]
  public void ShouldNotCrashWhenRenamingFileNotInHead()
  {
    // Scenario: File was renamed in history but then removed before HEAD
    // The main test is that it doesn't crash with KeyNotFoundException
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);

    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      // Commit 1: Add OldName.cs
      flow.Commit(commit =>
      {
        commit.Date(date1);
        commit.File("OldName.cs").By("Alice").Complexity(5).Added();
      });

      // Commit 2: Rename to NewName.cs
      flow.Commit(commit =>
      {
        commit.Date(date2);
        commit.File("OldName.cs").By("Bob").Complexity(7)
          .RenamedTo("NewName.cs");
      });

      // Commit 3 (HEAD): Add a different file
      flow.Commit(commit =>
      {
        commit.Date(date3);
        commit.File("DifferentFile.cs").By("Charlie").Complexity(1).Added();
      });
    });

    // Should complete without throwing KeyNotFoundException
    // Both files are in the final commit's history (OldName->NewName.cs and DifferentFile.cs)
    var entries = analysisResult.EntriesByDiminishingChangesCount().ToList();
    entries.Should().HaveCount(2, "should have both the renamed file and the different file");

    entries.Should().ContainSingle(e => e.PathOfCurrentVersion().ToString() == "NewName.cs",
      "the renamed file should be tracked under its new name");
    entries.Should().ContainSingle(e => e.PathOfCurrentVersion().ToString() == "DifferentFile.cs",
      "the different file should also be present");
  }

  [Test]
  public void ShouldHandleMultipleFilesWithMixedHistory()
  {
    // Complex scenario with multiple files having different histories
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);
    var date4 = new DateTime(2024, 1, 4);

    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      // Commit 1: Add FileA and FileB
      flow.Commit(commit =>
      {
        commit.Date(date1);
        commit.File("FileA.cs").By("Alice").Complexity(5).Added();
        commit.File("FileB.cs").By("Alice").Complexity(3).Added();
      });

      // Commit 2: Modify FileA, add FileC
      flow.Commit(commit =>
      {
        commit.Date(date2);
        commit.File("FileA.cs").By("Bob").Complexity(8).Modified();
        commit.File("FileC.cs").By("Bob").Complexity(2).Added();
      });

      // Commit 3: Modify FileB and FileC
      flow.Commit(commit =>
      {
        commit.Date(date3);
        commit.File("FileB.cs").By("Charlie").Complexity(6).Modified();
        commit.File("FileC.cs").By("Charlie").Complexity(4).Modified();
      });

      // Commit 4 (HEAD): Modify FileA
      flow.Commit(commit =>
      {
        commit.Date(date4);
        commit.File("FileA.cs").By("Diana").Complexity(12).Modified();
      });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount()
        .OrderBy(e => e.PathOfCurrentVersion().ToString())
        .ToList();
    
    entries.Should().HaveCount(3, "all three files should be in results");
    
    // FileA: Added + Modified + Modified = 3 changes
    var fileA = entries[0];
    fileA.PathOfCurrentVersion().Should().Be(RelativeFilePath("FileA.cs"));
    fileA.ChangesCount().Should().Be(3);
    fileA.ComplexityOfCurrentVersion().Should().Be(12);
    
    // FileB: Added + Modified = 2 changes
    var fileB = entries[1];
    fileB.PathOfCurrentVersion().Should().Be(RelativeFilePath("FileB.cs"));
    fileB.ChangesCount().Should().Be(2);
    fileB.ComplexityOfCurrentVersion().Should().Be(6);
    
    // FileC: Added + Modified = 2 changes
    var fileC = entries[2];
    fileC.PathOfCurrentVersion().Should().Be(RelativeFilePath("FileC.cs"));
    fileC.ChangesCount().Should().Be(2);
    fileC.ComplexityOfCurrentVersion().Should().Be(4);
  }

  [Test]
  public void ShouldStopProcessingFileAfterItsAdditionInHistory()
  {
    // Scenario: When going backward, after encountering a file's "add" point,
    // we should not process any older commits for that file
    var date1 = new DateTime(2024, 1, 1);
    var date2 = new DateTime(2024, 1, 2);
    var date3 = new DateTime(2024, 1, 3);

    var analysisResult = new RepoAnalysisDriver().Analyze(flow =>
    {
      // Commit 1 (oldest): Add FileB (FileA doesn't exist yet)
      flow.Commit(commit =>
      {
        commit.Date(date1);
        commit.File("FileB.cs").By("Alice").Complexity(5).Added();
      });

      // Commit 2: Add FileA
      flow.Commit(commit =>
      {
        commit.Date(date2);
        commit.File("FileA.cs").By("Bob").Complexity(10).Added();
      });

      // Commit 3 (HEAD): Modify both files
      flow.Commit(commit =>
      {
        commit.Date(date3);
        commit.File("FileA.cs").By("Charlie").Complexity(15).Modified();
        commit.File("FileB.cs").By("Charlie").Complexity(8).Modified();
      });
    });

    var entries = analysisResult.EntriesByDiminishingChangesCount().ToList();
    
    entries.Should().HaveCount(2);
    
    // FileA: Added in commit 2 + Modified in commit 3 = 2 changes
    var fileA = entries.First(e => e.PathOfCurrentVersion().ToString() == "FileA.cs");
    fileA.ChangesCount().Should().Be(2, "FileA was added in commit 2 and modified in commit 3");
    
    // FileB: Added in commit 1 + Modified in commit 3 = 2 changes  
    var fileB = entries.First(e => e.PathOfCurrentVersion().ToString() == "FileB.cs");
    fileB.ChangesCount().Should().Be(2, "FileB was added in commit 1 and modified in commit 3");
  }
}
