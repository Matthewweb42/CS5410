# 🎮 Flying Sword

A Flappy Bird-style game built with Godot 4.5 and C# where a sword flies through mountains.

## 🚀 Quick Start

1. **Open project** in Godot 4.5
2. **Build in VS Code**: Press `Ctrl+Shift+B` (you get IntelliSense + debugging!)
3. **Start coding**: Open `scripts/Player.cs` in VS Code, follow TODO comments
4. **Test in Godot**: Press F5 to run

💡 **With VS Code connected**, you get auto-complete, instant errors, and debugging! See [docs/VSCODE_WORKFLOW.md](docs/VSCODE_WORKFLOW.md)

## 📚 Documentation

All documentation is in the **[docs/](docs/)** folder.

**→ Start here: [docs/START_HERE.md](docs/START_HERE.md)** ⭐

**→ Full index: [docs/INDEX.md](docs/INDEX.md)** 📑

### Key Guides:
- **[VSCODE_WORKFLOW.md](docs/VSCODE_WORKFLOW.md)** 🔥 - VS Code + Godot workflow
- **[START_HERE.md](docs/START_HERE.md)** ⭐ - Main entry point
- **[QUICK_START.md](docs/QUICK_START.md)** - Get coding in 5 minutes
- **[CSHARP_GUIDE.md](docs/CSHARP_GUIDE.md)** - C# syntax reference
- **[IMPLEMENTATION_ORDER.md](docs/IMPLEMENTATION_ORDER.md)** - Step-by-step guide
- **[DATA_FLOW.md](docs/DATA_FLOW.md)** - Architecture diagrams

## 📁 Project Structure

```
flying-sword/
├── docs/              Documentation files
├── scripts/           C# scripts (implement these!)
│   ├── Player.cs
│   ├── Mountain.cs
│   ├── Floor.cs
│   ├── GameManager.cs
│   └── UI scripts...
├── scenes/            Godot scene files
│   ├── main.tscn
│   ├── player/
│   ├── mountain/
│   ├── floor/
│   └── ui/
├── assets/            Add your sprites and sounds here
│   ├── audio/
│   └── sprites/
└── project.godot      Main project file
```

## 🎯 What You'll Implement

- **Player**: Physics, input, collision, rotation
- **Mountain**: Scrolling obstacles, scoring zones
- **Floor**: Infinite scrolling ground
- **GameManager**: State machine, spawning, high scores
- **UI**: Start screen, game over screen, HUD

## 🎓 Learning Goals

- C# in Godot (signals, nodes, GetNode)
- Game architecture (states, data flow)
- Memory management (QueueFree)
- Input handling and collision detection
- File I/O for save data

## 📖 Next Steps

1. Read **[docs/START_HERE.md](docs/START_HERE.md)**
2. Build the project in Godot
3. Follow the TODO comments in each script
4. Test frequently and have fun!

---

**Made with Godot 4.5 and C#** | Educational Project
