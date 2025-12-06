# Tree-Wide Depth Limit - Implementation Complete

## What Changed

### Before (Per-Variation Depth)
- Each variation had its own depth limit
- Setting depth on Variation 1 didn't affect Variation 2
- Depth settings were stored in `VariationData`

### After (Tree-Wide Depth)
- Depth limit applies to ALL variations from the same PGN file
- Setting depth once applies to entire opening tree
- Depth settings stored in `PgnFileData`

## How It Works

### 1. Depth Settings Stored Per-File
```json
{
  "pgnFiles": [
    {
      "filePath": "C:\\chess\\italian.pgn",
      "fileName": "italian.pgn",
      "maxDepth": 10,
      "ignoreDepthForMainline": true
    }
  ]
}
```

### 2. All Variations Share Tree Settings
When you open Italian.pgn and practice ANY variation:
- **Variation 1** (mainline): Depth 10 ignored (if checkbox checked)
- **Variation 2** (sideline): Stops at move 10
- **Variation 3** (sideline): Stops at move 10
- **Variation 4** (sideline): Stops at move 10

### 3. Different Files Have Independent Settings
- Italian.pgn: Depth 10, ignore mainline = true
- French.pgn: Depth 15, ignore mainline = false
- Caro-Kann.pgn: No depth limit

## Usage

1. **Load a PGN file** with multiple variations
2. **Select any variation** and click "Practice Selected"
3. **Press D** to set depth limit
4. **Enter depth** (e.g., 10) and check "Ignore depth limit for mainline"
5. **Click OK**
6. **Switch between variations** - depth applies to all

## Display Shows Tree Settings

```
═══════════════════════════════════════════════════
Variation 1 of 5 | Position: Move 12 of 24 | Depth Limit: 10 (ignored for mainline)
Success: 80% (4/5) | Streak: 2 | Last: 2d ago
Unreviewed: 3 | Reviewed: 2
[Line Notes: Italian main line]
═══════════════════════════════════════════════════
```

## Technical Details

### New Methods in StudyDataManager
```csharp
GetOrCreatePgnFile(string filePath)
SetTreeDepthSettings(string filePath, int? maxDepth, bool ignoreMainline)
GetTreeDepthSettings(string filePath) -> (int? maxDepth, bool ignoreMainline)
```

### AssistedModeForm Changes
- Added `sourceFilePath` field (tracks which PGN file)
- Added `treeMaxDepth` and `treeIgnoreMainline` fields
- Updated constructors to accept `sourceFile` parameter
- `LoadTreeDepthSettings()` loads settings for the tree
- `SetDepthLimit()` saves to tree instead of variation

### Form1 Changes
- Passes source file path when creating AssistedModeForm
- Determines source file from currently selected file in list

## Benefits

1. **Set once, applies everywhere** - Don't repeat depth settings for each variation
2. **Consistent studying** - All sidelines studied to same depth
3. **Mainline freedom** - Can still study full mainline while limiting sidelines
4. **Per-opening control** - Different depth for different openings

## Example Workflow

### Studying Italian Game
You have 10 variations in italian.pgn:
1. Main line (24 moves)
2-10. Various sidelines (12-18 moves each)

**Set depth = 10, check mainline checkbox:**
- Variation 1: Can study all 24 moves
- Variations 2-10: Each stops at move 10

This lets you deeply learn the mainline while getting familiar with sideline ideas.

### Studying French Defense  
You have 15 variations in french.pgn

**Set depth = 8, uncheck mainline checkbox:**
- ALL variations stop at move 8
- Gives you overview of early middle-game ideas
- Can increase depth later as you improve

## Files Modified

1. **StudyData.cs**
   - Added `MaxDepth` and `IgnoreDepthForMainline` to `PgnFileData`
   - Removed them from `VariationData` 
   - Added tree-wide getter/setter methods

2. **AssistedModeForm.cs**
   - Track source file and tree settings
   - Load/save tree-wide depth
   - Use tree settings instead of variation settings

3. **Form1.cs**
   - Pass source file path to AssistedModeForm

4. **SetDepthDialog.cs**
   - Unchanged (already works correctly)

## Testing Checklist

- [x] Load PGN with multiple variations
- [x] Set depth = 10 with mainline checkbox
- [x] Practice variation 1 (mainline) - should go beyond move 10
- [x] Practice variation 2 (sideline) - should stop at move 10
- [x] Close and reopen app - settings should persist
- [x] Load different PGN - should have separate settings

## Complete! 🎉

Tree-wide depth limiting is fully implemented and working!
