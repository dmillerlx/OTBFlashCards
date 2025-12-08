# Priority System & Tree View - Complete!

## 🌟 New Priority Feature

### What It Does:
Mark variations as **Priority** to focus your study on important lines.

### Three Ways to Mark Priority:

**1. In Practice Mode (AssistedMode)**
- Press **T** while practicing a variation
- Toggles priority on/off
- Shows ⭐ PRIORITY in the status line

**2. In Tree View**
- Select a variation
- Click **⭐ Mark Priority** button
- Or click **☆ Unmark Priority** to remove

**3. Visual Indicators**
- **Priority variations** shown in **bold orange** (⭐)
- Low success (<50%) shown in red
- Medium success (<70%) shown in dark gold  
- High success (≥90%) shown in green

---

## 🌳 Tree View Window

### Access:
Click **🌳 Tree View** button on main window

### Features:

**1. See All Variations at a Glance**
```
Opening
├─ 1.e4 e5 2.Nf3 Nc6 3.Bc4... ⭐ [85% (17/20)]
├─ 1.e4 e5 2.Nf3 Nc6 3.Bb5... [60% (6/10)]
├─ 1.e4 c6 2.d4 d5 3.Nc3... [Not practiced]
└─ 1.e4 c5 2.Nf3 d6 3.d4... ⭐ [75% (12/16)]
```

**2. Color Coding**
- **Bold Orange** = Priority lines ⭐
- **Red** = <50% success (need work!)
- **Dark Gold** = 50-69% success  
- **Green** = ≥90% success (mastered!)
- **Black** = Not practiced yet

**3. Filter by Priority**
- Check **"Show Priority Lines Only"**
- See only your marked priority variations
- Great for focused practice sessions

**4. Stats Header**
```
Total Variations: 45 | Priority: 12 | Practiced: 32
```

**5. Actions**
- **⭐ Mark Priority** - Mark selected line as priority
- **☆ Unmark Priority** - Remove priority marking
- **Practice Selected** - Practice the selected variation
- **Double-click** any line to practice it immediately

---

## 📊 Metrics Window Updated

Priority variations now shown with ⭐ icon:
```
 85% (17/20) | Failed:  3 | Streak:  5 | Last: 2h ago   | ⭐ 1.e4 e5...
 60% ( 6/10) | Failed:  4 | Streak:  0 | Last: 1d ago   | 1.e4 c6...
```

---

## 🎯 Workflow Examples

### Example 1: Mark Your Main Lines as Priority
```
1. Open italian.pgn
2. Click 🌳 Tree View
3. Select main line variation
4. Click ⭐ Mark Priority
5. Repeat for critical sidelines
6. Check "Show Priority Lines Only"
7. Now see only your key lines!
```

### Example 2: Focus Practice Session
```
1. Click 🌳 Tree View
2. Check "Show Priority Lines Only"
3. Practice priority lines that need work
4. After mastering one, unmark it
5. Mark new lines to learn
```

### Example 3: Mark During Practice
```
1. Practicing a variation
2. Realize "this is important!"
3. Press T to mark as priority ⭐
4. Continue practicing
5. Later, view tree to see all priority lines
```

### Example 4: Track Progress on Priority Lines
```
1. Click 📊 Metrics
2. Sort by "Lowest Success Rate"
3. See priority lines (⭐) that need work
4. Practice them directly from metrics
```

---

## 🔑 Keyboard Shortcuts

### New in AssistedMode:
- **T** = Toggle Priority ⭐ (mark/unmark current variation)

### Complete Hotkeys:
```
NAVIGATION:
  Space/Right: Next move  |  Left: Previous move
  Home: Start  |  End: End of variation
  P/PageUp: Previous variation  |  N/PageDown: Next variation
  R: Random variation  |  U: Random unreviewed

MARKING & NOTES:
  F: Mark as Failed  |  S: Mark as Success
  T: Toggle Priority ⭐  |  L: Line notes  |  M: Move notes

OTHER:
  D: Set depth limit  |  H: Toggle help  |  Esc: Close
```

---

## 💾 Data Storage

Priority flag saved per variation in JSON:
```json
{
  "variations": {
    "abc123...": {
      "fullLine": "1.e4 e5 2.Nf3 Nc6 3.Bc4",
      "isPriority": true,
      "metrics": {
        "successRate": 85.0,
        "totalAttempts": 20
      }
    }
  }
}
```

---

## 🎨 Tree View Colors

| Color | Meaning | Example |
|-------|---------|---------|
| **Bold Orange** | Priority ⭐ | Important lines to master |
| **Red** | <50% success | Needs immediate attention |
| **Dark Gold** | 50-69% success | Getting there, keep practicing |
| **Green** | ≥90% success | Well practiced! |
| **Black** | Not practiced | Fresh material |

---

## 🚀 Use Cases

### Opening Preparation
- Mark your main repertoire lines as priority
- Filter to see only priority → focus practice
- Track which priority lines need more work

### Tournament Prep
- Week before: Mark opponent's likely lines as priority
- Practice only priority lines
- Check metrics to ensure 80%+ success on priority

### Learning New Opening
- Mark 3-4 most important variations as priority
- Master these first before branching out
- Once mastered (90%+), unmark and add new priorities

### Review Sessions
- Click Tree View → Show Priority Only
- See all your key lines at once
- Practice the ones with lower success rates

---

## ✅ Complete Feature Set

✅ **Mark variations as priority** (3 ways: practice mode, tree view, or during study)  
✅ **Visual indicators** (bold orange with ⭐ in all views)  
✅ **Filter by priority** (show only priority lines in tree view)  
✅ **Color-coded tree** (priority, success rate, not practiced)  
✅ **Quick toggle** (press T during practice)  
✅ **Statistics tracking** (see priority in metrics window)  
✅ **Persistent storage** (saved to JSON per variation)  

---

## 🎉 Result

You can now:
1. **Identify** your most important variations
2. **Mark them** as priority with one keystroke  
3. **Filter** to see only priority lines
4. **Focus** your practice time on what matters most
5. **Track** progress on priority variations
6. **Adjust** priorities as you improve

Perfect for systematic opening preparation! 🌟
