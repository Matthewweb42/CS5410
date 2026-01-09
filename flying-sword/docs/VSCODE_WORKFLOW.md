# VS Code + Godot Workflow

Since you have VS Code connected to Godot, your workflow is even better! Here's what's different.

## 🎯 Your Enhanced Workflow

### Setup (One-Time)
1. ✅ Godot project is open
2. ✅ VS Code is connected to Godot
3. ✅ C# solution is created

### Development Loop

#### Option 1: Build in VS Code (Recommended)
```
1. Edit code in VS Code
2. Save (Ctrl+S)
3. Build in VS Code: Ctrl+Shift+B
4. Switch to Godot window
5. Press F5 to run
6. See results instantly!
```

#### Option 2: Build in Godot
```
1. Edit code in VS Code
2. Save (Ctrl+S)
3. Switch to Godot window
4. Build: Project → Tools → C# → Build Solution
5. Press F5 to run
```

## 🔥 Hot Benefits of VS Code + Godot

### IntelliSense
- **Auto-completion** as you type
- **Parameter hints** for functions
- **Type checking** in real-time
- **Error highlighting** before you build

### Easy Navigation
- `Ctrl+Click` on a class/method to jump to definition
- `F12` - Go to definition
- `Shift+F12` - Find all references
- `Ctrl+P` - Quick file open

### Debugging
You can actually debug your C# code!

1. **Set breakpoints** in VS Code (click left of line number)
2. **In Godot**: Go to `Project → Tools → C# → Attach Debugger`
3. **Run game** in Godot (F5)
4. When code hits breakpoint, VS Code will pause
5. **Inspect variables**, step through code, etc.

### Multi-File Editing
- Split editor to see multiple files
- Search across entire project (Ctrl+Shift+F)
- Find and replace across files

## 🛠️ VS Code Extensions You Should Have

### Essential
- **C#** (Microsoft) - Language support
- **C# Dev Kit** (Microsoft) - Enhanced C# features
- **godot-tools** (Optional but helpful for .tscn files)

### Check Your Extensions
Press `Ctrl+Shift+X` in VS Code to see installed extensions.

## 📝 Recommended VS Code Settings

Create `.vscode/settings.json` in your project:

```json
{
  "files.exclude": {
    "**/.godot": true,
    "**/bin": true,
    "**/obj": true
  },
  "omnisharp.enableRoslynAnalyzers": true,
  "omnisharp.enableEditorConfigSupport": true
}
```

This hides build folders and enables better C# analysis.

## 🚀 Efficient Development Flow

### The Fast Loop (What You'll Do Most)

```
VS Code                          Godot
┌────────────────────┐          ┌────────────────────┐
│ 1. Edit Player.cs  │          │                    │
│ 2. Save (Ctrl+S)   │          │                    │
│ 3. Build (Ctrl+⇧+B)│ ────────>│ 4. Auto-reloads    │
│                    │          │ 5. Press F5 to run │
│                    │ <────────│ 6. See results     │
│ 7. Fix issues      │          │                    │
└────────────────────┘          └────────────────────┘
```

### When You Have Errors

**VS Code shows errors immediately** with red squiggles:
- Hover over red squiggle to see error
- Check Problems panel (Ctrl+Shift+M)
- Fix and save
- Build again

**No need to switch to Godot** until code compiles!

## 💡 Pro Tips

### 1. Use TODO Comments
VS Code highlights TODO comments:
```csharp
// TODO: Implement flap function
private void Flap()
{
    // Your code here
}
```

See all TODOs: Search for `TODO` (Ctrl+Shift+F)

### 2. Use Snippets
Type common patterns faster:
- `prop` → Property
- `ctor` → Constructor
- `cw` → Console.WriteLine

### 3. Format on Save
Add to settings.json:
```json
{
  "editor.formatOnSave": true
}
```

### 4. Auto Save
Never forget to save:
```json
{
  "files.autoSave": "afterDelay",
  "files.autoSaveDelay": 1000
}
```

### 5. Keep Both Windows Visible
- **Dual monitors**: Godot on one, VS Code on other
- **Single monitor**: Use Alt+Tab to switch quickly
- **Or**: Snap windows side-by-side (Win+Left, Win+Right)

## 🐛 Debugging Example

Let's debug the Player flap:

1. **In VS Code**, open `scripts/Player.cs`
2. **Click left of line number** in `Flap()` function (red dot appears)
3. **In Godot**: `Project → Tools → C# → Attach Debugger`
   - Select "VS Code" if prompted
4. **Run game** in Godot (F5)
5. **Click to flap** - VS Code will pause at breakpoint!
6. **Inspect**:
   - Hover over `_velocityY` to see value
   - Look at Variables panel
   - Check call stack
7. **Step through**:
   - F10 - Step over (next line)
   - F11 - Step into (enter function)
   - F5 - Continue
8. **Stop**: Shift+F5 or click Stop button

## 🔄 Common Workflows

### Testing a Single Function

```
1. Write function in VS Code
2. Add GD.Print() statements for debugging
3. Save and build (Ctrl+S, Ctrl+Shift+B)
4. Switch to Godot, press F6 to run current scene
5. Check output in Godot's Output panel
6. Adjust and repeat
```

### Implementing a Feature

```
1. Read TODO comments in VS Code
2. Implement function
3. Use IntelliSense for correct types
4. Save and build
5. Test in Godot
6. Use debugger if something's wrong
7. Iterate until working
```

## ⚡ Keyboard Shortcuts Reference

### VS Code
- `Ctrl+S` - Save
- `Ctrl+Shift+B` - Build
- `F12` - Go to definition
- `Ctrl+.` - Quick fix
- `Ctrl+Space` - Trigger IntelliSense
- `Ctrl+Shift+M` - Show problems
- `Ctrl+P` - Quick open file
- `Ctrl+Shift+F` - Search in files

### Godot
- `F5` - Run project
- `F6` - Run current scene
- `F7` - Run with debugger (for C# breakpoints)
- `Ctrl+Q` - Quit running game

## 📊 Your Advantage

With VS Code + Godot connected, you get:
- ✅ **Faster development** - Build in VS Code
- ✅ **Better errors** - See them as you type
- ✅ **Code navigation** - Jump around easily
- ✅ **Debugging** - Set breakpoints, inspect variables
- ✅ **IntelliSense** - Auto-complete everything
- ✅ **Refactoring** - Rename, extract methods easily

## 🎯 Best Practice Workflow

```
Morning/Start of Session:
1. Open Godot project
2. Open VS Code (should auto-connect)
3. Build in VS Code (Ctrl+Shift+B) to verify setup

During Development:
1. Code in VS Code
2. Save frequently (or enable auto-save)
3. Build in VS Code after each change
4. Switch to Godot to test
5. Use debugger when confused

End of Session:
1. Make final build in VS Code
2. Test one last time in Godot
3. Close VS Code, then Godot
```

## 🆘 Troubleshooting

### "VS Code not showing IntelliSense"
- Check C# extension is installed
- Reload window: `Ctrl+Shift+P` → "Reload Window"
- Check .csproj file exists
- Try: `Ctrl+Shift+P` → "OmniSharp: Restart OmniSharp"

### "Build fails in VS Code but works in Godot"
- Make sure you're building the correct project
- Check build output in VS Code Terminal
- Try building in Godot first, then VS Code

### "Debugger won't attach"
- Make sure game is running in Godot (F5 or F7)
- Try: Godot → `Project → Tools → C# → Attach Debugger`
- Select correct debugger (VS Code)
- Check firewall isn't blocking connection

### "Changes not appearing in game"
- Did you save? (Ctrl+S)
- Did you build? (Ctrl+Shift+B)
- Check build succeeded (no errors)
- Try: Close game in Godot, rebuild, run again

---

**You have the best of both worlds!** Code with VS Code's power, test with Godot's engine. 🚀
