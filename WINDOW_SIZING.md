# OTB Flash Cards - Window Sizing and Positioning

## Main Window (File Manager)
- **Size**: 1100 x 650 pixels
- **Position**: Center of screen on startup
- **Laptop Friendly**: Yes - fits comfortably on 1366x768 screens (common laptop resolution)

## Assisted Mode Window
- **Size**: 900 x 650 pixels
- **Position**: Centered relative to parent window
- **Minimum Size**: 700 x 500 pixels (user can resize if needed)
- **Laptop Friendly**: Yes - smaller than main window, fits well on laptops

## Screen Compatibility

### Common Laptop Resolutions
✅ **1920x1080 (Full HD)** - Plenty of room
✅ **1600x900** - Comfortable fit
✅ **1366x768** (Most common laptop screen) - Works well
  - Main: 1100x650 leaves margins of 133px on sides, 59px top/bottom
  - Assisted: 900x650 has even more room

### Vertical Space
- 650px height works well because:
  - Typical laptop: 768px - taskbar (40-60px) = ~700px usable
  - 650px leaves room for window title bar and taskbar
  - Compact help mode (H key) reduces clutter for smaller screens

## Design Decisions

### Main Window (1100x650)
- Three columns need horizontal space for readability
- 650px height allows good list visibility without scrolling
- CenterScreen on startup for immediate visibility

### Assisted Mode (900x650)
- Single column of text needs less width than three panels
- 900px width allows ~80-100 characters per line
- Comments display properly without excessive wrapping
- CenterParent - opens relative to main window
- Resizable with 700x500 minimum for flexibility

## Tips for Small Screens

1. **Use compact help mode (H key)** - Reduces footer from 9 lines to 1 line
2. **Maximize if needed** - Both windows are resizable
3. **Main window** - All three panels have proper anchoring and scrollbars
4. **Assisted mode** - Text scrolls naturally with auto-scroll to bottom

## Testing Recommendations

Test on these resolutions:
- 1920x1080 (desktop/modern laptop)
- 1366x768 (common budget laptop)
- 1280x720 (older/small laptops)

The current sizing should work well on all of these!
