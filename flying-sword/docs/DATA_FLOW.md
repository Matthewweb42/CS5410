# Data Flow Reference

## Signal Flow (Child → Parent, UP the tree)

### Player → Game Manager
```
Player.gd                          Game Manager.gd
├─ player_scored ──────────────→  _on_player_scored()
│                                   └─ Increment score
│                                   └─ Play sound
│                                   └─ Update HUD
│
└─ player_died ────────────────→  _on_player_died()
                                    └─ Play sound
                                    └─ Call stop_game()
```

### Mountain Score Zone → (Detected by Player)
```
Mountain.gd                        Player.gd
└─ ScoreZone.area_entered ─────→  _on_area_entered(area)
                                    └─ Check if score zone
                                    └─ Emit player_scored signal ──→ Game Manager
```

### Spawn Timer → Game Manager
```
Timer                              Game Manager.gd
└─ timeout ────────────────────→  _on_spawn_timer_timeout()
                                    └─ spawn_mountain()
```

## Method Call Flow (Parent → Child, DOWN the tree)

### Game Manager Controls Everything
```
Game Manager.gd
│
├─→ Player.reset()
│    └─ Reset position, velocity, rotation
│
├─→ Floor.start_scrolling()
│    └─ Enable scrolling
│
├─→ Floor.stop_scrolling()
│    └─ Disable scrolling
│
├─→ Floor.reset()
│    └─ Reset scroll offset
│
├─→ HUD.update_score(score)
│    └─ Update score label text
│
├─→ HUD.show_hud() / hide_hud()
│    └─ Toggle visibility
│
├─→ StartScreen.show_screen() / hide_screen()
│    └─ Toggle visibility
│
└─→ GameOverScreen.show_screen(score, high_score)
     └─ Update labels and show
```

## Game State Transitions

### START → PLAYING (start_game)
1. Game Manager detects click in START state
2. Calls start_game()
   - Set state to PLAYING
   - Reset score to 0
   - Player input enabled (is_alive = true)
   - Start SpawnTimer
   - Call Floor.start_scrolling()
   - Call HUD.show_hud()
   - Call StartScreen.hide_screen()

### PLAYING → GAME_OVER (collision occurs)
1. Player collides with mountain/floor
2. Player calls own die() method
   - Set is_alive = false (disables input)
   - Emit player_died signal ──→ Game Manager
3. Game Manager receives signal
4. Calls stop_game()
   - Set state to GAME_OVER
   - Stop SpawnTimer
   - Call Floor.stop_scrolling()
   - Update high score if needed
   - Save high score to file
   - Call GameOverScreen.show_screen(score, high_score)
   - Call HUD.hide_hud()

### GAME_OVER → START (restart)
1. Game Manager detects click in GAME_OVER state
2. Calls restart_game()
   - Call cleanup_mountains() (remove all mountain instances)
   - Call Player.reset()
   - Call Floor.reset()
   - Set state to START
   - Call GameOverScreen.hide_screen()
   - Call StartScreen.show_screen()

## Object Lifecycle

### Mountain Spawning
```
Game Manager spawns mountains:
1. SpawnTimer timeout every N seconds
2. _on_spawn_timer_timeout() called
3. spawn_mountain() creates instance
4. Instance added to scene tree
5. Mountain scrolls left each frame
6. When off-screen, Mountain.queue_free() removes itself
```

### Mountain Cleanup (Critical!)
```
Two cleanup mechanisms:

1. Normal (during gameplay):
   Mountain._process(delta)
   └─ Check is_off_screen()
      └─ If true: queue_free()

2. On restart (cleanup all):
   Game Manager.cleanup_mountains()
   └─ Get all mountain nodes
      └─ Call queue_free() on each
```

## Input Flow

### During START State
```
Click → Game Manager._input()
       └─ Check: current_state == START?
          └─ Yes: Call start_game()
```

### During PLAYING State
```
Click → Player._process()
       └─ Check: is_alive == true?
          └─ Yes: Call flap()
             ├─ Set velocity_y = FLAP_STRENGTH
             ├─ Play flap sound
             └─ Continue...
```

### During GAME_OVER State
```
Click → Game Manager._input()
       └─ Check: current_state == GAME_OVER?
          └─ Yes: Call restart_game()
```

## Collision Detection Flow

### Mountain Collision
```
Player overlaps Mountain
└─→ Player._on_area_entered(area)
    └─ Check: is it a mountain?
       └─ Yes: Call die()
          ├─ Set is_alive = false
          ├─ Play death sound
          └─ Emit player_died ──→ Game Manager._on_player_died()
```

### Floor Collision
```
Player overlaps Floor
└─→ Player._on_body_entered(body)
    └─ Check: is it the floor?
       └─ Yes: Call die()
          └─ (same as above)
```

### Score Zone Collision
```
Player enters Mountain Score Zone
└─→ Player._on_area_entered(area)
    └─ Check: is it a score zone?
       └─ Yes: Emit player_scored ──→ Game Manager._on_player_scored()
          └─ Increment current_score
          └─ Play score sound
          └─ Update HUD
```

## Why This Architecture?

### Signals UP = Decoupling
- Player doesn't need to know Game Manager exists
- Player just says "I died!" or "I scored!"
- Any node can listen to these signals
- Easy to add new listeners (e.g., achievements system)

### Methods DOWN = Control
- Game Manager has clear control over game flow
- Parent nodes manage child state
- Easy to understand who controls what
- Clear ownership of game objects

### Scene Instancing = Reusability
- Mountain scene is a template
- Spawn as many as needed
- Each has its own state
- All share same behavior
- Clean memory management with queue_free()
