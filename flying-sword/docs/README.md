# Flying Sword - Flappy Bird Style Game

A Godot 4.5 C# implementation of a Flappy Bird-style game where a sword flies through mountains.

## Project Structure

```
flying-sword/
├── scenes/
│   ├── main.tscn              # Main game scene (ties everything together)
│   ├── player/
│   │   └── player.tscn        # Sword player scene
│   ├── mountain/
│   │   └── mountain.tscn      # Mountain obstacle scene
│   ├── floor/
│   │   └── floor.tscn         # Scrolling floor scene
│   └── ui/
│       ├── start_screen.tscn  # Start screen UI
│       ├── game_over_screen.tscn  # Game over screen UI
│       └── hud.tscn           # In-game HUD (score display)
├── scripts/
│   ├── GameManager.cs         # Main game logic and state management
│   ├── Player.cs              # Player movement, input, and collision
│   ├── Mountain.cs            # Mountain scrolling and scoring
│   ├── Floor.cs               # Floor scrolling
│   ├── StartScreen.cs         # Start screen display logic
│   ├── GameOverScreen.cs      # Game over screen display logic
│   └── HUD.cs                 # HUD display logic
├── assets/
│   ├── audio/                 # Sound effects (flap, score, death)
│   └── sprites/               # Sprites for sword, mountains, floor, background
└── project.godot              # Godot project configuration
```

## Data Flow Architecture

### Signals (Data flows UP the scene tree)
- **Player → Game Manager**:
  - `PlayerDied` - Emitted when player collides with mountain or floor
  - `PlayerScored` - Emitted when player passes through mountain gap

- **Mountain Score Zone → Player**:
  - Area entered signal triggers scoring

### Method Calls (Data flows DOWN the scene tree)
- **Game Manager → Player**:
  - `Reset()` - Reset player position and state

- **Game Manager → UI Screens**:
  - `ShowScreen()` / `HideScreen()` - Control visibility
  - `ShowScreen(score, highScore)` - Update game over screen with scores

- **Game Manager → HUD**:
  - `UpdateScore(score)` - Update score display

- **Game Manager → Floor**:
  - `StartScrolling()` / `StopScrolling()` - Control floor movement
  - `Reset()` - Reset floor to initial state

## Game States

The game has three states managed by `GameManager.cs`:

1. **START**: Initial state, showing start screen
   - Waiting for player input to begin
   - Mountains not spawning
   - Player not moving

2. **PLAYING**: Active gameplay
   - Player can flap/move
   - Mountains spawning at intervals
   - Floor scrolling
   - Score tracking active

3. **GAME_OVER**: After collision
   - Showing game over screen with scores
   - Waiting for player input to restart
   - Mountains stop spawning
   - Floor stops scrolling

## Implementation Guide

### Step 1: Start with Constants
Each script has constants at the top marked with `TODO`. Fill these in first:
- Physics values (gravity, flap strength)
- Speeds (scroll speed, rotation speed)
- Positions (spawn positions, boundaries)
- Sizes (gap size, floor width)

### Step 2: Implement Core Functions

#### Player (Player.cs)
1. `_Ready()` - Get node references, connect signals, set up collision
2. `_PhysicsProcess()` - Apply gravity, update position, check ceiling
3. `_Process()` - Check for input using `Input.IsActionJustPressed()`
4. `Flap()` - Apply upward velocity, play sound
5. `UpdateRotation()` - Rotate based on velocity using `Mathf.Lerp()`
6. `Die()` - Disable input, emit signal using `EmitSignal()`
7. `OnAreaEntered()` - Handle mountain/score zone collision
8. `OnBodyEntered()` - Handle floor collision

#### Mountain (Mountain.cs)
1. `_Ready()` - Get node references, position top/bottom mountains with gap
2. `_Process()` - Move left, check if off-screen
3. `SetGapPosition()` - Position mountains around gap
4. `OnScoreZoneAreaEntered()` - Detect player scoring

#### Floor (Floor.cs)
1. `_Ready()` - Get node reference, set initial position
2. `_Process()` - Scroll texture infinitely using modulo
3. `StartScrolling()` / `StopScrolling()` - Control movement

#### Game Manager (GameManager.cs)
1. `_Ready()` - Load mountain scene, load high score, get references, connect signals
2. `_Input()` - Handle clicks based on current state using `InputEvent`
3. `StartGame()` - Transition START → PLAYING
4. `StopGame()` - Transition PLAYING → GAME_OVER
5. `RestartGame()` - Clean up and return to START
6. `SpawnMountain()` - Instantiate and add mountain using `Instantiate<Mountain>()`
7. `CleanupMountains()` - Remove all mountains (prevents memory leaks!)
8. `OnPlayerScored()` - Increment score, update UI
9. `OnPlayerDied()` - Play sound, stop game
10. `SaveHighScore()` / `LoadHighScore()` - Persistent data using `FileAccess`

#### UI Scripts
Simple show/hide and text update functions.

### Step 3: Set Up Collision Layers
In Godot editor, configure:
- **Layer 1 (Player)**: Player Area2D
- **Layer 2 (Obstacles)**: Mountains and Floor
- **Layer 3 (Score Zones)**: Mountain score zones

Player should:
- Be on Layer 1
- Detect Layers 2 and 3

### Step 4: Add Assets
Place your sprites and sounds in the `assets/` folders and assign them in the scene editor:
- Sword sprite → Player AnimatedSprite2D
- Mountain sprites → Mountain Sprite2Ds
- Floor texture → Floor Sprite2D (set to repeat)
- Sounds → AudioStreamPlayer2D nodes

### Step 5: Connect Signals in Code
For C#, signals are connected in the `_Ready()` method:
```csharp
// In GameManager._Ready()
_player.PlayerDied += OnPlayerDied;
_player.PlayerScored += OnPlayerScored;
_spawnTimer.Timeout += OnSpawnTimerTimeout;

// In Player._Ready()
AreaEntered += OnAreaEntered;
BodyEntered += OnBodyEntered;

// In Mountain._Ready()
_scoreZone.AreaEntered += OnScoreZoneAreaEntered;
```

## Key Concepts

### Why This Structure?
- **Separation of Concerns**: Each scene handles its own behavior
- **Reusability**: Mountains are instanced, not duplicated
- **Clean Communication**: Signals up, method calls down
- **Memory Safety**: Proper cleanup prevents leaks

### Physics Without Physics Engine
This game uses simple manual physics:
```csharp
_velocityY += Gravity * (float)delta;  // Apply gravity
Position = new Vector2(Position.X, Position.Y + _velocityY * (float)delta);  // Update position
```
This is simpler than RigidBody2D and sufficient for this game.

### Infinite Scrolling
Floor and background use texture offset with modulo:
```csharp
_scrollOffset = (_scrollOffset + ScrollSpeed * (float)delta) % FloorWidth;
```

### Mountain Cleanup
Critical to prevent memory leaks:
- Mountains check `IsOffScreen()` each frame
- When off-screen, call `QueueFree()`
- On restart, manually `QueueFree()` any remaining mountains

## Testing Checklist
- [ ] Player flaps when clicking
- [ ] Player rotates based on velocity
- [ ] Player dies on mountain collision
- [ ] Player dies on floor collision
- [ ] Player bounces off ceiling (doesn't die)
- [ ] Score increments when passing mountains
- [ ] High score saves and loads
- [ ] Mountains spawn at intervals
- [ ] Mountains are cleaned up off-screen
- [ ] Game resets properly (no memory leaks)
- [ ] All sounds play at correct times
- [ ] UI screens transition correctly

## Next Steps
1. Fill in all TODO comments with actual implementation
2. Test each function individually
3. Add sprites and sounds
4. Tune constants for good game feel
5. Optional: Add parallax background scrolling

Good luck learning Godot with C#! 🎮

## C# Specific Notes

### Getting Node References
```csharp
_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
```

### Connecting Signals
```csharp
_player.PlayerDied += OnPlayerDied;
```

### Emitting Signals
```csharp
EmitSignal(SignalName.PlayerDied);
```

### Instantiating Scenes
```csharp
var mountain = _mountainScene.Instantiate<Mountain>();
AddChild(mountain);
```

### File I/O
```csharp
using var file = FileAccess.Open("user://highscore.save", FileAccess.ModeFlags.Write);
file.StoreVar(_highScore);
```

### Type Conversions
- Cast `double delta` to `float` when needed: `(float)delta`
- Convert int to string: `score.ToString()`
- Convert Variant to int: `.AsInt32()`
