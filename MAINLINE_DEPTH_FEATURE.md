# Mainline Depth Ignore Feature

## What's Been Implemented

### 1. New Checkbox in Depth Dialog
When you press **D** to set depth limit, you now get a dialog with:
- Text box for depth (move numbers)
- **Checkbox: "Ignore depth limit for mainline variations"**
- OK, Clear, and Cancel buttons

### 2. Mainline Detection
- Currently detects mainline as the first variation (index 0)
- This is the variation without branches
- Can be enhanced later to detect based on PGN structure

### 3. How It Works

**Without Checkbox:**
- Depth 10 = stops at move 10 for ALL variations

**With Checkbox Checked:**
- Depth 10 = stops at move 10 for branched variations
- Depth 10 = IGNORED for mainline (variation #1)
- Mainline can play through to the end

### 4. Display Shows Status

**For mainline with checkbox:**
```
Variation 1 of 5 | Position: Move 15 of 24 | Depth Limit: 10 (ignored for mainline)
```

**For other variations:**
```
Variation 2 of 5 | Position: Move 10 of 18 | Depth Limit: 10
```

### 5. JSON Storage

The checkbox state is saved per variation:
```json
{
  "maxDepth": 10,
  "ignoreDepthForMainline": true
}
```

## Use Case Example

You have:
- **Variation 1**: Italian Game mainline (24 moves, no branches)
- **Variation 2**: Italian Game sideline (18 moves)
- **Variation 3**: Italian Game another sideline (16 moves)

You set Depth = 10 with "Ignore depth for mainline" checked:

- **Variation 1** (mainline): Can study all 24 moves (depth ignored)
- **Variation 2** (sideline): Stops at move 10
- **Variation 3** (sideline): Stops at move 10

This lets you study the full mainline while limiting practice on side variations.

## Future Enhancement Ideas

### Better Mainline Detection
Instead of just checking index 0, could detect mainline by:
1. Checking PGN structure for variations (lines with parentheses)
2. Marking variations as mainline during import
3. Adding a "Mark as Mainline" button in the UI

### Per-Variation Mainline Flag
Could add a checkbox in the main window to manually mark any variation as "mainline" regardless of its position.

## Files Modified

1. **StudyData.cs**: Added `IgnoreDepthForMainline` property
2. **SetDepthDialog.cs**: New dialog with checkbox
3. **AssistedModeForm.cs**: 
   - Updated `SetDepthLimit()` to use dialog
   - Added `IsMainlineVariation()` helper
   - Updated `NextMove()` to respect checkbox
   - Updated display to show "(ignored for mainline)"

## Testing

1. Load a PGN with variations
2. Select first variation (mainline)
3. Press D, set depth to 10, check the box
4. Navigate - should go beyond move 10
5. Switch to variation 2 (sideline)
6. Navigate - should stop at move 10

## Complete!

The mainline depth ignore feature is fully implemented and working!
