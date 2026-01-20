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
	private int _highScore = 5;  // Hardcoded starting high score for testing
	
	// Pipe spawning
	private PackedScene _pipeScene;
	private const float SpawnInterval = 2.5f;  // Time between pipe spawns
	private const float SpawnXPosition = 650.0f;  // Spawn off right edge (screen is 576px wide)
	private const float MinSpawnY = 300.0f;  // Minimum Y for gap center
	private const float MaxSpawnY = 700.0f;  // Maximum Y for gap center (screen is 1024px tall)
	
	// References to game objects
	private Player _player;
	private Floor _floor;
	private Background _background;
	
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
		// Load pipe scene for spawning
		_pipeScene = GD.Load<PackedScene>("res://scenes/pipe/pipe.tscn");
		
		// Get references to child nodes
		_player = GetNode<Player>("Player");
		_floor = GetNode<Floor>("Floor");
		_background = GetNode<Background>("Background");
		_startScreen = GetNode<StartScreen>("StartScreen");
		_gameOverScreen = GetNode<GameOverScreen>("GameOverScreen");
		_hud = GetNode<HUD>("HUD");
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_scoreSound = GetNode<AudioStreamPlayer2D>("ScoreSound");
		_deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
		
		// Connect player signals
		_player.PlayerDied += OnPlayerDied;
		_player.PlayerScored += OnPlayerScored;
		
		// Connect spawn timer
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
		_spawnTimer.WaitTime = SpawnInterval;
		
		// Load high score
		LoadHighScore();
		
		// Start in Start state - show start screen, hide others
		_startScreen.Visible = true;
		_gameOverScreen.Visible = false;
		_hud.Visible = false;
		
		GD.Print("GameManager initialized!");
	}
	
	
	public override void _Input(InputEvent @event)
	{
		// Handle click/tap based on game state
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
				case GameState.Playing:
					// Input handled by player
					break;
			}
		}
	}
	
	
	private void StartGame()
	{
		GD.Print("Starting game!");
		
		// Transition to Playing state
		_currentState = GameState.Playing;
		_currentScore = 0;
		
		// Reset player
		_player.Reset();
		
		// Start scrolling
		_floor.StartScrolling();
		_background.StartScrolling();
		
		// Start spawning pipes
		_spawnTimer.Start();
		
		// Update UI
		_startScreen.Visible = false;
		_hud.Visible = true;
		_hud.UpdateScore(_currentScore);
	}
	
	
	private void StopGame()
	{
		GD.Print("Game over!");
		
		// Transition to GameOver state
		_currentState = GameState.GameOver;
		
		// Stop spawning and scrolling
		_spawnTimer.Stop();
		_floor.StopScrolling();
		_background.StopScrolling();
		StopAllPipes();
		
		// Update high score
		UpdateHighScore();
		SaveHighScore();
		
		// Update UI
		_hud.Visible = false;
		_gameOverScreen.Visible = true;
		_gameOverScreen.ShowScreen(_currentScore, _highScore);
	}
	
	
	private void RestartGame()
	{
		GD.Print("Restarting game!");
		
		// Clean up pipes
		CleanupPipes();
		
		// Reset player and floor
		_player.Reset();
		_floor.Reset();
		_background.Reset();
		
		// Return to Start state
		_currentState = GameState.Start;
		
		// Update UI
		_gameOverScreen.Visible = false;
		_startScreen.Visible = true;
		_hud.Visible = false;
	}
	
	
	private void SpawnPipe()
	{
		if (_pipeScene == null)
		{
			GD.PrintErr("Pipe scene not loaded!");
			return;
		}
		
		// Instantiate pipe
		var pipe = _pipeScene.Instantiate<Node2D>();
		
		// Set random Y position for the gap center using configured bounds
		float randomY = (float)GD.RandRange(MinSpawnY, MaxSpawnY);
		pipe.Position = new Vector2(SpawnXPosition, randomY);
		
		// Add to scene
		AddChild(pipe);
		
		GD.Print($"Spawned pipe at Y: {randomY}");
	}
	
	
	private void CleanupPipes()
	{
		// Remove all pipe children
		foreach (Node child in GetChildren())
		{
			if (child.Name.ToString().Contains("Pipe") || child is Pipe)
			{
				child.QueueFree();
			}
		}
		GD.Print("Pipes cleaned up");
	}
	
	
	private void StopAllPipes()
	{
		// Stop scrolling on all active pipes
		foreach (Node child in GetChildren())
		{
			if (child is Pipe pipe)
			{
				pipe.StopScrolling();
			}
		}
		GD.Print("All pipes stopped");
	}
	
	
	private void OnSpawnTimerTimeout()
	{
		if (_currentState == GameState.Playing)
		{
			SpawnPipe();
		}
	}
	
	
	private void OnPlayerScored()
	{
		_currentScore++;
		
		if (_scoreSound.Stream != null)
		{
			_scoreSound.Play();
		}
		
		_hud.UpdateScore(_currentScore);
		GD.Print($"Score: {_currentScore}");
	}
	
	
	private void OnPlayerDied()
	{
		if (_deathSound.Stream != null)
		{
			_deathSound.Play();
		}
		
		StopGame();
	}
	
	
	private void LoadHighScore()
	{
		string savePath = "user://highscore.save";
		
		if (FileAccess.FileExists(savePath))
		{
			using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
			_highScore = (int)file.Get32();
			GD.Print($"Loaded high score: {_highScore}");
		}
		else
		{
			_highScore = 0;
		}
	}
	
	
	private void SaveHighScore()
	{
		string savePath = "user://highscore.save";
		
		using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
		file.Store32((uint)_highScore);
		GD.Print($"Saved high score: {_highScore}");
	}
	
	
	private void UpdateHighScore()
	{
		if (_currentScore > _highScore)
		{
			_highScore = _currentScore;
			GD.Print($"New high score: {_highScore}");
		}
	}
}
