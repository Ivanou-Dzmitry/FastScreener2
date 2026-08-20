# FastScreener2

A lightweight Windows screen capture and annotation tool. Capture a defined area of your screen, annotate it with arrows, frames, numbers, and text, then save or copy to clipboard — all without leaving your workflow.

**Version:** 2.0.4.6 · .NET 8 · Windows

---

## Features

### Capture
- Define up to **4 preset capture window sizes** via the toolbar
- Capture window **snaps to screen corners** automatically
- Captures the current monitor; supports **multi-monitor setups with different DPI**
- Output saved to file and/or clipboard simultaneously

### Annotation Tools
All tools are activated with the **Middle Mouse Button (MMB)**.

| Tool | Description |
|------|-------------|
| **Arrow** | Click to place a directional arrow. Right-click to rotate. |
| **Frame** | Highlight a region. *Free* mode: drag to draw. *Fixed* mode: click to place at preset size. |
| **Number** | Place sequential numbers. Counter resets on each new capture. Right-click the button to change font family. |
| **Text** | Add a single line of text (up to 45 characters). |
| **Watermark** | Composite a logo or image onto every saved screenshot. Right-click the button to select the image file. |

- **Undo** last annotation with `Ctrl+Z`
- **Bars** at top and bottom of the capture window mask unwanted screen areas

### Guide Lines
Non-destructive overlay lines to help with composition (not saved to screenshot).
- **3x3** — rule of thirds
- **4x4** — equal quarters  
- **Custom** — arbitrary pixel offsets from each edge; Lock Indent syncs all four sides

### Settings & Profiles
All visual parameters (colors, sizes, arrow style, font, etc.) are configured in the **Settings** window.

**Profiles** let you save and load named setting presets — useful when different projects need different annotation styles or output folders.

- Each profile is stored as an independent XML file in the `profiles/` subfolder
- Switch profiles from the dropdown at the bottom of the Settings window

### File Output
- **Format:** PNG or JPG
- **PNG Depth:** 32bpp (default) / 24bpp (no alpha, ~25% smaller) / 8bpp (256-color palette, smallest)
- **JPEG Compression:** quality 1–100 (default 75)
- **Save Folder:** configurable per profile; defaults to a `screenshots/` folder next to the executable
- **File naming:** fixed name (overwrites on each capture) or auto-generated timestamped name

### DPI & Multi-Monitor
- **DPI Scale** setting scales annotation element sizes with the monitor's scale factor
- Settings window adapts automatically when dragged between monitors with different DPI

---

## Requirements

- Windows 10 / 11
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (self-contained build includes it)

---

## Getting Started

1. Launch `FastScreener2.exe`
2. Resize and position the capture window over the area you want to capture
3. Use MMB to add annotations
4. Press the capture button or hotkey to save/copy the screenshot

**Troubleshooting:** If the app fails to start, delete `fs2_settings.xml` from the application folder. A fresh settings file will be created on next launch.

---

## Configuration Files

| File | Description |
|------|-------------|
| `fs2_settings.xml` | Active settings |
| `profiles/<name>.xml` | Named setting presets |
| `screenshots/` | Default output folder (created automatically) |

---

## Author

Dzmitry Ivanou · [id.cgtalk@gmail.com](mailto:id.cgtalk@gmail.com)
