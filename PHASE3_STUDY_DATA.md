# Study Data & Metrics Implementation - Phase 3

## Overview
This phase adds comprehensive tracking of practice attempts, metrics, notes, and depth limiting using JSON storage.

## Files Created

### 1. StudyData.cs
Complete JSON-based data model and management system including:
- **StudyData**: Root data structure
- **PgnFileData**: Track PGN files
- **VariationData**: Store variation info, attempts, notes, metrics
- **AttemptData**: Individual practice attempts
- **MetricsData**: Aggregated statistics
- **StudyDataManager**: Load/save JSON, generate line hashes, manage data

### 2. SettingsForm.cs / SettingsForm.Designer.cs
Dialog for configuring:
- Study data file location (JSON file path)
- Browse button to select location
- Save/Cancel buttons

## Key Features Implemented

### JSON Data Structure
```json
{
  "configVersion": 1,
  "pgnFiles": [
    {
      "filePath": "C:\\path\\to\\file.pgn",
      "fileName": "file.pgn",
      "dateAdded": "2025-12-05T10:30:00"
    }
  ],
  "variations": {
    "ABC123...": {
      "lineHash": "ABC123...",
      "fullLine": "1.e4 e5 2.Nf3 Nc6 3.Bb5",
      "moveCount": 5,
      "maxDepth": 10,
      "lineNotes": "Main Spanish opening line",
      "moveNotes": {
        "2": "Key move - develop knight",
        "4": "Spanish opening starts here"
      },
      "attempts": [
        {
          "date": "2025-12-05T14:00:00",
          "success": true,
          "depthReached": 10,
          "notes": ""
        }
      ],
      "metrics": {
        "totalAttempts": 5,
        "successfulAttempts": 4,
        "failedAttempts": 1,
        "lastAttemptDate": "2025-12-05T14:00:00",
        "successRate": 80.0,
        "currentStreak": 2
      }
    }
  }
}
```

### Unique Line Identification
- Uses **SHA256 hash** of the full move sequence
- Same variation across different files = same tracking
- Hash stored as 32-character Base64 string
- Full line also stored for human readability

### Metrics Tracked
- **Total Attempts**: How many times practiced
- **Successful Attempts**: Completed without marking as failed
- **Failed Attempts**: User pressed F to mark as failed
- **Success Rate**: Percentage (successful / total * 100)
- **Last Attempt Date**: When last practiced
- **Current Streak**: Consecutive successes

### Helper Methods
- `GenerateLineHash()`: Create unique hash from move list
- `GetOrCreateVariation()`: Get existing or create new variation data
- `RecordAttempt()`: Log a practice attempt and update metrics
- `GetFailedVariations()`: Get variations where last attempt was a failure
- `GetLowSuccessRateVariations(threshold)`: Get variations below success rate threshold

## UI Changes Required

### 1. Add Settings Button to Main Form
In Form1.Designer.cs, add to panelTop after buttonPractice:
```csharp
//
// buttonSettings  
//
buttonSettings.Location = new Point(410, 15);
buttonSettings.Name = "buttonSettings";
buttonSettings.Size = new Size(100, 30);
buttonSettings.TabIndex = 3;
buttonSettings.Text = "⚙ Settings";
buttonSettings.UseVisualStyleBackColor = true;
buttonSettings.Click += new EventHandler(buttonSettings_Click);

// Add to panelTop.Controls
panelTop.Controls.Add(buttonSettings);
```

### 2. Update Form1 (Already Done)
- Added `CheckFirstRun()` - shows settings dialog on first run
- Added `ShowSettings()` - opens settings dialog
- Added `buttonSettings_Click()` handler

### 3. Update Program.cs (Already Done)
- Added `StudyDataManager.Initialize()` call on startup

## Next Steps: Enhance AssistedModeForm

The following features need to be added to AssistedModeForm.cs:

### A. Track Current Variation Data
```csharp
private VariationData currentVariationData;

// In constructor or when variation changes:
currentVariationData = StudyDataManager.GetOrCreateVariation(variation);
```

### B. Add Depth Limiting
```csharp
private int GetEffectiveDepth()
{
    if (currentVariationData.MaxDepth.HasValue)
        return Math.Min(currentMoveIndex + 1, currentVariationData.MaxDepth.Value);
    return currentMoveIndex + 1;
}

// In NextMove():
if (currentVariationData.MaxDepth.HasValue && 
    currentMoveIndex >= currentVariationData.MaxDepth.Value - 1)
{
    // Reached depth limit
    MarkCurrentAsReviewed();
    MessageBox.Show($"Depth limit reached ({currentVariationData.MaxDepth})");
}
```

### C. Add New Keyboard Shortcuts
```csharp
case Keys.F:
    MarkAsFailed();
    return true;

case Keys.D:
    SetDepthLimit();
    return true;

case Keys.L:
    EditLineNotes();
    return true;

case Keys.M:
    EditMoveNotes();
    return true;
```

### D. Record Attempt on Exit
```csharp
protected override void OnFormClosing(FormClosingEventArgs e)
{
    // Record attempt (assume success unless F was pressed)
    StudyDataManager.RecordAttempt(
        currentVariationData,
        !markedAsFailed,  // success
        currentMoveIndex + 1,  // depth reached
        ""
    );
    base.OnFormClosing(e);
}
```

### E. Display Metrics
Update the status display to show:
```
Variation 2 of 5 | Position: Move 5 of 14 | Depth Limit: 10
Success: 80% (4/5) | Streak: 2 | Last: 2 days ago
```

### F. Show Notes
Display line notes and move notes (if they exist) in the variation display.

## Filter/Sort Variations (Future)

Add to main Form1 for filtering variations list:
```csharp
// Dropdown filters
- Show All
- Never Practiced
- Last Attempt Failed
- Success Rate < 70%
- Success Rate < 50%
- Not Practiced in 7 days

// Button: "Practice Failed Lines" (like Random button)
// Button: "Practice Weak Spots" (low success rate)
```

## Testing the JSON System

1. **First Run**: App will prompt for data file location
2. **Practice a Variation**: Attempt will be recorded
3. **Check JSON File**: Open the file to see the data
4. **Practice Again**: Metrics should update
5. **Mark as Failed**: Press F, check that failedAttempts increments

## Example JSON After Practice

```json
{
  "configVersion": 1,
  "pgnFiles": [],
  "variations": {
    "xY7zAb...": {
      "lineHash": "xY7zAb...",
      "fullLine": "1.e4 e5 2.Nf3 Nc6 3.Bc4 Nf6 4.Ng5 d5",
      "moveCount": 8,
      "maxDepth": null,
      "lineNotes": "",
      "moveNotes": {},
      "attempts": [
        {
          "date": "2025-12-05T15:30:00",
          "success": true,
          "depthReached": 8,
          "notes": ""
        }
      ],
      "metrics": {
        "totalAttempts": 1,
        "successfulAttempts": 1,
        "failedAttempts": 0,
        "lastAttemptDate": "2025-12-05T15:30:00",
        "successRate": 100.0,
        "currentStreak": 1
      }
    }
  }
}
```

## Next Implementation Priority

1. ✅ JSON data structure
2. ✅ Settings dialog
3. ✅ First-run experience
4. ⏳ Add Settings button to Form1 (manual in Visual Studio)
5. ⏳ Track attempts in AssistedModeForm
6. ⏳ Add F key to mark failed
7. ⏳ Add D key for depth limiting
8. ⏳ Add L/M keys for notes
9. ⏳ Display metrics in practice mode
10. ⏳ Add "Random Failed" button to main form

Ready to proceed with the AssistedModeForm enhancements?
