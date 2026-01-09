# Implementation Order Guide

Follow this order to build the game incrementally and test as you go.

## Phase 1: Basic Player Movement (Test: Can the sword move?)

### 1.1 Player Physics (player.cs)
Fill in constants first:
- GRAVITY = 1000.0
- FLAP_STRENGTH = -400.0
- MAX_FALL_SPEED = 500.0

Then implement _physics_process, _process, and flap functions.

Test: Open player.tscn, press F6. Sword should fall due to gravity, flap upward when clicking.

### 1.2 Player Rotation (player.cs)
Add rotation constants and implement update_rotation function.

Test: Sword should tilt up when flapping, down when falling.

### 1.3 Ceiling Check (player.cs)
Implement check_ceiling_collision function.

Test: Sword cannot fly above screen top.

---

## Phase 2: Floor Setup (Test: Does floor scroll?)

### 2.1 Floor Scrolling (floor.cs)
Set SCROLL_SPEED and implement _process to move floor left.

Test: Floor should move left when game runs.

---

## Phase 3: Mountain Spawning (Test: Do mountains appear?)

### 3.1 Mountain Setup (mountain.cs)
Implement set_gap_position and _process for scrolling.

Test: Open mountain.tscn, press F6. Mountains should scroll left.

### 3.2 Game Manager Spawning (game_manager.cs)
Implement spawn_mountain and _on_spawn_timer_timeout.

Test: Mountains should spawn every 2 seconds at right edge.

---

## Phase 4: Collisions (Test: Does player die on hit?)

### 4.1 Collision Setup (In Godot Editor)
1. Add CollisionShape2D to all collision objects
2. Set collision layers properly:
   - Player: layer 1, mask 2|3
   - Mountains/Floor: layer 2
   - Score zones: layer 3

### 4.2 Player Collision Handlers (player.cs)
Implement _on_area_entered and _on_body_entered.

Test: Player should die when hitting mountains or floor.

---

## Phase 5: Game States (Test: Can you start/stop/restart?)

### 5.1 State Management (game_manager.cs)
Implement _input, start_game, stop_game, restart_game, and cleanup_mountains.

Test: Should be able to start, play, die, see game over, restart.

---

## Phase 6: Scoring (Test: Does score increase?)

### 6.1 Score Zone (mountain.cs)
Implement _on_score_zone_area_entered.

### 6.2 Score Tracking (game_manager.cs)
Implement _on_player_scored.

### 6.3 UI Updates (hud.cs, game_over_screen.cs)
Implement update_score and show_screen functions.

Test: Score should increment when passing through mountains.

---

## Phase 7: High Score Persistence (Test: Does high score save?)

### 7.1 Save/Load (game_manager.cs)
Implement load_high_score, save_high_score, and update_high_score.

Test: High score should persist between game sessions.

---

## Phase 8: Audio (Test: Do sounds play?)

### 8.1 Add Audio Files
Import sound files to assets/audio/ and assign to AudioStreamPlayer2D nodes.

### 8.2 Play Sounds
Add sound.play() calls in flap, _on_player_scored, and _on_player_died.

Test: Sounds should play on flap, score, and death.

---

## Phase 9: Polish and Tuning

### 9.1 Add Sprites
Import sprites and assign to Sprite2D nodes.

### 9.2 Tune Constants
Adjust GRAVITY, FLAP_STRENGTH, SCROLL_SPEED, etc. for better game feel.

### 9.3 Background
Add background sprite or ColorRect. Optional: parallax scrolling.

---

## Testing Checklist

After each phase:
- [ ] No errors in console
- [ ] Expected behavior works
- [ ] No memory leaks (watch Node count)
- [ ] Game can restart multiple times
- [ ] Performance is smooth (60 FPS)

## Common Issues

- Mountains do not spawn: Check spawn timer starts in start_game()
- Player does not die: Check collision layers/masks
- Memory leak: Ensure cleanup_mountains() removes all instances
- Score does not save: Check file path permissions
- Floor does not loop: Duplicate floor tiles or use region_rect with offset

Good luck!
