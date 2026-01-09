# Quick Start Guide - Flying Sword (C#)

## Setup

1. **Open Project in Godot 4.5**
   - Open Godot
   - Import the project.godot file
   - Wait for initial import to complete

2. **Create C# Solution** (if not already created)
   - Go to Project → Tools → C# → Create C# solution
   - This generates the .sln and .csproj files

3. **Build the Project**
   - Go to Project → Tools → C# → Build Solution
   - Or press Ctrl+Shift+B (Windows) / Cmd+Shift+B (Mac)
   - Fix any errors that appear in the build output

## Project Structure Overview

```
scripts/
├── Player.cs          - Sword physics, input, collision
├── Mountain.cs        - Obstacle scrolling and scoring
├── Floor.cs           - Infinite scrolling floor
├── GameManager.cs     - Main game logic, state, spawning
├── StartScreen.cs     - Start UI
├── GameOverScreen.cs  - Game over UI
└── HUD.cs            - In-game score display
```

## Implementation Order

Follow [IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md) but with C# syntax.

### Phase 1: Player Movement (Start Here!)

Open `scripts/Player.cs` and implement:

1. **Fill in constants:**
```csharp
private const float Gravity = 1000.0f;
private const float FlapStrength = -400.0f;
private const float MaxFallSpeed = 500.0f;
```

2. **Get node references in _Ready():**
```csharp
public override void _Ready()
{
    _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    _flapSound = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    
    AreaEntered += OnAreaEntered;
    BodyEntered += OnBodyEntered;
}
```

3. **Implement physics in _PhysicsProcess():**
```csharp
public override void _PhysicsProcess(double delta)
{
    if (!_isAlive) return;
    
    // Apply gravity
    _velocityY += Gravity * (float)delta;
    _velocityY = Mathf.Min(_velocityY, MaxFallSpeed);
    
    // Update position
    var pos = Position;
    pos.Y += _velocityY * (float)delta;
    Position = pos;
}
```

4. **Test:** Run player.tscn (F6) - sword should fall!

### Phase 2: Input

Still in Player.cs:

```csharp
public override void _Process(double delta)
{
    if (Input.IsActionJustPressed("flap") && _isAlive)
    {
        Flap();
    }
}

private void Flap()
{
    _velocityY = FlapStrength;
    // _flapSound.Play();  // Add later when you have audio
}
```

**Test:** Sword should jump when you click!

### Phase 3: Continue Implementation

Follow the TODO comments in each script. Key files to implement in order:
1. `scripts/Player.cs` - Movement & collision
2. `scripts/Floor.cs` - Scrolling floor
3. `scripts/Mountain.cs` - Obstacles
4. `scripts/GameManager.cs` - Game flow
5. UI scripts - Display & transitions

## Testing Checklist

After implementing each phase, verify:
- [ ] No build errors (check bottom panel in Godot)
- [ ] No runtime errors (check debugger)
- [ ] Expected behavior works
- [ ] Can run main scene (F5)

## Common First-Time Issues

### "Cannot find type or namespace"
- Build the project: Project → Tools → C# → Build Solution
- Check that class names match file names (case-sensitive)

### "Node not found"
- Verify node paths in GetNode<T>() calls
- Open the scene file (.tscn) to see node hierarchy
- Node names are case-sensitive

### "Property is read-only"
- Position, Rotation are read-only in C#
- Must create new Vector2: `Position = new Vector2(x, y)`

### Nothing happens when I run the game
- Check if _Ready() is being called (add GD.Print("Ready called"))
- Verify scripts are attached to nodes in scene files
- Build the project before running

## Adding Assets

1. **Sprites:**
   - Place images in `assets/sprites/`
   - Open scene file (e.g., player.tscn)
   - Select Sprite2D or AnimatedSprite2D node
   - Drag texture to Texture property

2. **Sounds:**
   - Place audio files in `assets/audio/`
   - Select AudioStreamPlayer2D node
   - Drag audio file to Stream property

## Resources

- [README.md](README.md) - Full project documentation
- [CSHARP_GUIDE.md](CSHARP_GUIDE.md) - C# syntax reference
- [DATA_FLOW.md](DATA_FLOW.md) - How signals and methods connect
- [IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md) - Step-by-step guide
- [Godot C# API Docs](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/index.html)

## Next Steps

1. Implement Player movement (Phase 1)
2. Test after each function
3. Move to Floor, then Mountains
4. Implement GameManager state machine
5. Add UI
6. Add assets (sprites, sounds)
7. Tune constants for fun gameplay!

Good luck! Remember: implement one function at a time, test frequently, and read the TODO comments carefully.
