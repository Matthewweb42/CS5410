using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	// Game states
	private enum GameState { Start, Playing, GameOver }
	private GameState _currentState = GameState.Start;
	
	// Score tracking
	private int _currentScore = 0;
	private int _highScore = 0;
	
	// Mountain spawning
	private PackedScene _mountainScene;  // TODO: Load in _Ready() using GD.Load<PackedScene>()
	private const float SpawnInterval = 0.0f;  // TODO: Set time between mountain spawns in seconds
	private const float SpawnXPosition = 0.0f;  // TODO: Set x position where mountains spawn (off right edge)
	
	// References to game objects
	private Player _player;
	private Floor _floor;
	
	// UI references
	private StartScreen _startScreen;
	private GameOverScreen _gameOverScreen;
	private HUD _hud;
	
	// Spawning
	private Timer _spawnTimer;
	
	// Audio
	private AudioStreamPlayer2D _scoreSound;
	private AudioStreamPlayer2D _deathSound;
	
	
	public override void _Ready()
	{
		// TODO: Initialize game manager
		// - Load _mountainScene using GD.Load<PackedScene>("res://scenes/mountain/mountain.tscn")
		// - Load high score using LoadHighScore()
		// - Get references to child nodes using GetNode<T>()
		// - Connect player signals: _player.PlayerDied += OnPlayerDied
		// - Connect player signals: _player.PlayerScored += OnPlayerScored
		// - Connect spawn timer: _spawnTimer.Timeout += OnSpawnTimerTimeout
		// - Set up spawn timer wait_time to SpawnInterval
		// - Start in Start state
		// - Show start screen, hide other UI
	}
	
	
	public override void _Input(InputEvent @event)
	{
		// TODO: Handle input based on game state
		// - Check if event is InputEventMouseButton and Pressed property is true
		// - Use switch statement on _currentState:
		//   - GameState.Start: call StartGame()
		//   - GameState.GameOver: call RestartGame()
		//   - GameState.Playing: input is handled by player
	}
	
	
	private void StartGame()
	{
		// TODO: Transition from Start to Playing
		// - Set _currentState to GameState.Playing
		// - Reset _currentScore to 0
		// - Call _player.Reset()
		// - Set player._isAlive to true (you may need to make this public or use a method)
		// - Start spawning mountains using _spawnTimer.Start()
		// - Start floor scrolling using _floor.StartScrolling()
		// - Hide start screen using _startScreen.HideScreen()
		// - Show and update HUD
	}
	
	
	private void StopGame()
	{
		// TODO: Transition from Playing to GameOver
		// - Set _currentState to GameState.GameOver
		// - Stop spawning mountains using _spawnTimer.Stop()
		// - Stop floor scrolling using _floor.StopScrolling()
		// - Call UpdateHighScore()
		// - Call SaveHighScore()
		// - Hide HUD
		// - Show game over screen with scores
	}
	
	
	private void RestartGame()
	{
		// TODO: Reset everything and return to Start state
		// - Call CleanupMountains() to remove all mountains
		// - Call _player.Reset()
		// - Call _floor.Reset()
		// - Set _currentState to GameState.Start
		// - Hide game over screen
		// - Show start screen
	}
	
	
	private void SpawnMountain()
	{
		// TODO: Create and spawn a new mountain
		// - Instantiate _mountainScene using _mountainScene.Instantiate<Mountain>()
		// - Set random y position for gap using GD.RandfRange(min, max)
		// - Set x position to SpawnXPosition
		// - Set mountain.Position = new Vector2(x, y)
		// - Add to scene tree using AddChild(mountain)
	}
	
	
	private void CleanupMountains()
	{
		// TODO: Remove all mountains from scene
		// - Get all children using GetChildren()
		// - Loop through children
		// - Check if child is Mountain type using "is Mountain"
		// - Call QueueFree() on each mountain
		// - Important for preventing memory leaks!
	}
	
	
	private void OnSpawnTimerTimeout()
	{
		// TODO: Called when spawn timer triggers
		// - If _currentState == GameState.Playing, call SpawnMountain()
	}
	
	
	private void OnPlayerScored()
	{
		// TODO: Handle player scoring a point
		// - Increment _currentScore
		// - Play score sound using _scoreSound.Play()
		// - Update HUD with new score using _hud.UpdateScore(_currentScore)
	}
	
	
	private void OnPlayerDied()
	{
		// TODO: Handle player death
		// - Play death sound using _deathSound.Play()
		// - Call StopGame()
	}
	
	
	private void LoadHighScore()
	{
		// TODO: Load high score from save file
		// - Use FileAccess.FileExists() to check if save file exists
		// - If exists, use FileAccess.Open() with FileAccess.ModeFlags.Read
		// - Read high score using file.GetVar().AsInt32()
		// - Close file using file.Close() or Dispose()
		// - If file doesn't exist, set _highScore = 0
		// - Save path: "user://highscore.save"
	}
	
	
	private void SaveHighScore()
	{
		// TODO: Save high score to file
		// - Open file using FileAccess.Open() with FileAccess.ModeFlags.Write
		// - Write _highScore using file.StoreVar(_highScore)
		// - Close file
		// - Save path: "user://highscore.save"
	}
	
	
	private void UpdateHighScore()
	{
		// TODO: Check and update high score
		// - If _currentScore > _highScore:
		//   - Set _highScore = _currentScore
	}
}
