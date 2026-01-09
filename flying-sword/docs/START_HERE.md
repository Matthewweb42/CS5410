# 🎮 FLYING SWORD - START HERE

Welcome! This is your **Flappy Bird-style game** where a sword flies through mountains.

## ⚡ Quick Start (3 Steps)

### 1️⃣ Open Project
- Launch **Godot 4.5**
- Open `project.godot`

### 2️⃣ Build C# Solution
- **In VS Code** (recommended): Press `Ctrl+Shift+B`
- **Or in Godot**: `Project → Tools → C# → Build Solution`
- Wait for build to complete
- ✨ **VS Code gives you IntelliSense, debugging, and instant error checking!**

### 3️⃣ Start Implementing
- Open `scripts/Player.cs` **in VS Code**
- Follow the TODO comments (auto-complete will help!)
- Build in VS Code (`Ctrl+Shift+B`)
- Test in Godot (press `F5`)
- 🔥 **See [VSCODE_WORKFLOW.md](VSCODE_WORKFLOW.md) for pro tips!**

## 📚 Documentation Guide

Read these in order (all in the `docs/` folder):

1. **[VSCODE_WORKFLOW.md](VSCODE_WORKFLOW.md)** 🔥 - VS Code + Godot workflow (MUST READ!)
2. **[QUICK_START.md](QUICK_START.md)** ⭐ - Detailed first steps and Phase 1 implementation
3. **[CSHARP_GUIDE.md](CSHARP_GUIDE.md)** - C# syntax reference for Godot
4. **[IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md)** - Complete step-by-step build guide
5. **[Full README](README.md)** - Full project documentation and architecture
6. **[DATA_FLOW.md](DATA_FLOW.md)** - How signals and methods connect
7. **[ARCHITECTURE.txt](ARCHITECTURE.txt)** - Visual diagrams

## 📁 What's In This Project

### Your Code (Implement These):
```
scripts/
├── Player.cs           - Sword movement, input, collision
├── Mountain.cs         - Obstacles that scroll left
├── Floor.cs            - Infinite scrolling ground
├── GameManager.cs      - Game states and spawning
├── StartScreen.cs      - Start menu
├── GameOverScreen.cs   - Game over screen
└── HUD.cs             - Score display
```

### Scenes (Edit in Godot):
```
scenes/
├── main.tscn           - Main game scene
├── player/player.tscn  - Sword player
├── mountain/mountain.tscn - Mountain obstacle
├── floor/floor.tscn    - Scrolling floor
└── ui/                 - UI screens
```

### Assets (Add Your Files Here):
```
assets/
├── audio/    - Add .wav or .ogg sound files
└── sprites/  - Add .png image files
```

## 🎯 What You'll Learn

- **C# in Godot**: Syntax, signals, nodes
- **Game Architecture**: States, spawning, cleanup
- **Data Flow**: Signals up (events), methods down (control)
- **Memory Management**: Preventing leaks with QueueFree()
- **Input Handling**: Mouse clicks and keyboard
- **File I/O**: Saving/loading high scores

## 🔨 Implementation Tips

### Start Small
- Implement one function at a time
- Test after each function
- Don't try to do everything at once

### Follow the TODOs
Each function has detailed comments:
```csharp
private void Flap()
{
    // TODO: Make the player flap
    // - Set _velocityY to FlapStrength
    // - Play flap sound using _flapSound.Play()
}
```

### Build Often
- Press `Ctrl+Shift+B` to build
- Fix errors before running
- Check the Output panel for errors

### Test Frequently
- `F5` - Run main scene
- `F6` - Run current scene
- `Ctrl+Q` - Quit game

## 🚀 First Implementation (5 Minutes)

Open `scripts/Player.cs` and try this:

### Step 1: Add Constants
```csharp
private const float Gravity = 1000.0f;
private const float FlapStrength = -400.0f;
```

### Step 2: Implement _PhysicsProcess
```csharp
public override void _PhysicsProcess(double delta)
{
    _velocityY += Gravity * (float)delta;
    var pos = Position;
    pos.Y += _velocityY * (float)delta;
    Position = pos;
}
```

### Step 3: Test It!
- Build: `Ctrl+Shift+B`
- Open `scenes/player/player.tscn`
- Press `F6` to run
- **You should see the player falling!** 🎉

## 🆘 Common Issues

**"Cannot find type"**
- Build the project first: `Ctrl+Shift+B`

**"Node not found"**
- Check GetNode<T>() paths match scene hierarchy
- Use exact node names (case-sensitive)

**Nothing happens**
- Add `GD.Print("Test")` to check if code runs
- Check Output panel for errors

**Build errors**
- Read error message carefully
- Check for missing semicolons, brackets
- Ensure all classes are `public partial class`

## 🎓 Learning Path

1. **Week 1**: Get player moving and flapping
2. **Week 2**: Add mountains and collisions
3. **Week 3**: Implement game states and UI
4. **Week 4**: Add scoring, high scores, polish

## 📖 Need Help?

- Check the documentation files listed above
- Read TODO comments in the code
- Use `GD.Print()` for debugging
- Check Godot C# docs: https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/

---

**Ready? Open [QUICK_START.md](QUICK_START.md) for detailed first steps!**

Good luck and have fun! 🚀🎮
