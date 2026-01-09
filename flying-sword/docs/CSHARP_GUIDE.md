# C# Implementation Guide for Godot

This guide covers C#-specific patterns you'll use in this project.

## Key Differences from GDScript

### 1. Class Declaration
```csharp
public partial class Player : Area2D  // Must be "partial" for Godot C#
{
    // Class members
}
```

### 2. Node References
```csharp
// GDScript: @onready var sprite = $AnimatedSprite2D
// C#:
private AnimatedSprite2D _animatedSprite;

public override void _Ready()
{
    _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
}
```

### 3. Signals

**Declaring:**
```csharp
[Signal]
public delegate void PlayerDiedEventHandler();

[Signal]
public delegate void PlayerScoredEventHandler();
```

**Emitting:**
```csharp
EmitSignal(SignalName.PlayerDied);
```

**Connecting:**
```csharp
_player.PlayerDied += OnPlayerDied;
_spawnTimer.Timeout += OnSpawnTimerTimeout;
```

### 4. Overriding Godot Methods
```csharp
public override void _Ready() { }
public override void _Process(double delta) { }
public override void _PhysicsProcess(double delta) { }
public override void _Input(InputEvent @event) { }
```

### 5. Input Handling
```csharp
// Check if action just pressed
if (Input.IsActionJustPressed("flap") && _isAlive)
{
    Flap();
}

// Check mouse button
if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
{
    // Handle click
}
```

### 6. Vector2 Operations
```csharp
// GDScript: position.x += 10
// C# (Position is read-only, must create new Vector2):
Position = new Vector2(Position.X + 10, Position.Y);

// Or use shorthand:
var pos = Position;
pos.X += 10;
Position = pos;
```

### 7. Math Functions
```csharp
// Use Mathf class instead of math functions
float clamped = Mathf.Clamp(value, min, max);
float lerped = Mathf.Lerp(from, to, weight);
float random = GD.RandfRange(min, max);
float radians = Mathf.DegToRad(degrees);
```

### 8. Loading Resources
```csharp
private PackedScene _mountainScene;

public override void _Ready()
{
    _mountainScene = GD.Load<PackedScene>("res://scenes/mountain/mountain.tscn");
}
```

### 9. Instantiating Scenes
```csharp
var mountain = _mountainScene.Instantiate<Mountain>();
mountain.Position = new Vector2(600, 400);
AddChild(mountain);
```

### 10. File I/O
```csharp
// Save
private void SaveHighScore()
{
    using var file = FileAccess.Open("user://highscore.save", FileAccess.ModeFlags.Write);
    file.StoreVar(_highScore);
}

// Load
private void LoadHighScore()
{
    if (FileAccess.FileExists("user://highscore.save"))
    {
        using var file = FileAccess.Open("user://highscore.save", FileAccess.ModeFlags.Read);
        _highScore = file.GetVar().AsInt32();
    }
}
```

### 11. Type Conversions
```csharp
// delta is double, often need float
float deltaFloat = (float)delta;

// int to string
string scoreText = _currentScore.ToString();

// Variant to specific type
int score = variant.AsInt32();
float value = variant.AsSingle();
```

### 12. Checking Node Types
```csharp
if (area.GetParent() is Player player)
{
    // It's a player
}

// Or using name
if (area.Name == "ScoreZone")
{
    // It's a score zone
}
```

### 13. Getting Children
```csharp
foreach (Node child in GetChildren())
{
    if (child is Mountain mountain)
    {
        mountain.QueueFree();
    }
}
```

### 14. Constants
```csharp
private const float ScrollSpeed = 200.0f;  // Use 'f' suffix for float literals
private const float SpawnInterval = 2.0f;
```

## Common Patterns in This Project

### Pattern 1: Initialize Node References
```csharp
private Player _player;
private Floor _floor;

public override void _Ready()
{
    _player = GetNode<Player>("Player");
    _floor = GetNode<Floor>("Floor");
}
```

### Pattern 2: Connect Signals
```csharp
public override void _Ready()
{
    _player.PlayerDied += OnPlayerDied;
    _player.PlayerScored += OnPlayerScored;
}

private void OnPlayerDied()
{
    // Handle death
}

private void OnPlayerScored()
{
    // Handle score
}
```

### Pattern 3: Update Position
```csharp
public override void _Process(double delta)
{
    var pos = Position;
    pos.X -= ScrollSpeed * (float)delta;
    Position = pos;
}
```

### Pattern 4: State Machine
```csharp
private enum GameState { Start, Playing, GameOver }
private GameState _currentState = GameState.Start;

public override void _Input(InputEvent @event)
{
    if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
    {
        switch (_currentState)
        {
            case GameState.Start:
                StartGame();
                break;
            case GameState.GameOver:
                RestartGame();
                break;
        }
    }
}
```

## Debugging Tips

### Print Statements
```csharp
GD.Print("Score: " + _currentScore);
GD.Print($"Position: {Position}");  // String interpolation
```

### Check for Null
```csharp
if (_player == null)
{
    GD.PrintErr("Player reference is null!");
    return;
}
```

### Try-Catch for File Operations
```csharp
try
{
    using var file = FileAccess.Open("user://save.dat", FileAccess.ModeFlags.Read);
    _data = file.GetVar().AsInt32();
}
catch (Exception e)
{
    GD.PrintErr($"Failed to load: {e.Message}");
}
```

## Building the Project

1. Open project in Godot
2. Go to Project → Tools → C# → Create C# solution
3. Build the project: Project → Tools → C# → Build Solution
4. Or use external IDE (Visual Studio, Rider, VS Code)

## Common Errors and Solutions

### Error: "Partial class missing"
- Make sure class is declared as `public partial class`

### Error: "Cannot convert double to float"
- Cast delta: `(float)delta`

### Error: "Property is read-only"
- Position, Rotation, etc. are read-only - create new Vector2/float

### Error: "Signal not found"
- Make sure signal is declared with `[Signal]` attribute
- Use correct event handler suffix: `EventHandler`

### Error: "Node not found"
- Check node path in GetNode<T>()
- Ensure node exists in scene tree
- Use exact node name (case-sensitive)

Good luck!
