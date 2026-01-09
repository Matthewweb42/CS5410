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
	private PackedScene _mountainScene;
	private const float SpawnInterval = 2.0f;  // Time between mountain spawns
	private const float SpawnXPosition = 650.0f;  // Spawn off right edge of 576px screen
	
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
		// Load mountain scene for spawning
		_mountainScene = GD.Load<PackedScene>("res://scenes/mountain/mountain.tscn");
		
		// Get references to child nodes
		_player = GetNode<Player>("Player");
		_floor = GetNode<Floor>("Floor");
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
		
		// Start floor scrolling
		_floor.StartScrolling();
		
		// Start spawning mountains
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
		
		// Clean up mountains
		CleanupMountains();
		
		// Reset player and floor
		_player.Reset();
		_floor.Reset();
		
		// Return to Start state
		_currentState = GameState.Start;
		
		// Update UI
		_gameOverScreen.Visible = false;
		_startScreen.Visible = true;
		_hud.Visible = false;
	}
	
	
	private void SpawnMountain()
	{
		if (_mountainScene == null)
		{
			GD.PrintErr("Mountain scene not loaded!");
			return;
		}
		
		// Instantiate mountain
		var mountain = _mountainScene.Instantiate<Node2D>();
		
		// Set random Y position for the gap (between 200 and 700)
		float randomY = (float)GD.RandRange(250, 650);
		mountain.Position = new Vector2(SpawnXPosition, randomY);
		
		// Add to scene
		AddChild(mountain);
		
		GD.Print($"Spawned mountain at Y: {randomY}");
	}
	
	
	private void CleanupMountains()
	{
		// Remove all mountain children
		foreach (Node child in GetChildren())
		{
			if (child.Name.ToString().Contains("Mountain") || child is Mountain)
			{
				child.QueueFree();
			}
		}
		GD.Print("Mountains cleaned up");
	}
	
	
	private void OnSpawnTimerTimeout()
	{
		if (_currentState == GameState.Playing)
		{
			SpawnMountain();
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
