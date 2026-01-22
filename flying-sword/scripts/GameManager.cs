using Godot;
using System;

public partial class GameManager : Node
{
	private enum GameState { Start, Playing, GameOver }
	private GameState _currentState = GameState.Start;
	
	private int _currentScore = 0;
	private int _highScore = 0;
	
	private PackedScene _pipeScene;
	private const float SpawnInterval = 2.5f;
	private const float SpawnXPosition = 650.0f;
	private const float MinSpawnY = 300.0f;
	private const float MaxSpawnY = 700.0f;
	
	private Player _player;
	private Floor _floor;
	private Background _background;
	private StartScreen _startScreen;
	private GameOverScreen _gameOverScreen;
	private HUD _hud;
	private Timer _spawnTimer;
	private AudioStreamPlayer2D _scoreSound;
	private AudioStreamPlayer2D _deathSound;
	
	
	public override void _Ready()
	{
		_pipeScene = GD.Load<PackedScene>("res://scenes/pipe/pipe.tscn");
		
		_player = GetNode<Player>("Player");
		_floor = GetNode<Floor>("Floor");
		_background = GetNode<Background>("Background");
		_startScreen = GetNode<StartScreen>("StartScreen");
		_gameOverScreen = GetNode<GameOverScreen>("GameOverScreen");
		_hud = GetNode<HUD>("HUD");
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_scoreSound = GetNode<AudioStreamPlayer2D>("ScoreSound");
		_deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
		
		_player.PlayerDied += OnPlayerDied;
		_player.PlayerScored += OnPlayerScored;
		
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
		_spawnTimer.WaitTime = SpawnInterval;
		
		LoadHighScore();
		
		_startScreen.Visible = true;
		_gameOverScreen.Visible = false;
		_hud.Visible = false;
	}
	
	
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
	
	
	private void StartGame()
	{
		_currentState = GameState.Playing;
		_currentScore = 0;
		
		_player.StartPlaying();
		_floor.StartScrolling();
		_background.StartScrolling();
		_spawnTimer.Start();
		
		_startScreen.Visible = false;
		_hud.Visible = true;
		_hud.UpdateScore(_currentScore);
	}
	
	
	private void StopGame()
	{
		_currentState = GameState.GameOver;
		
		_spawnTimer.Stop();
		_floor.StopScrolling();
		_background.StopScrolling();
		StopAllPipes();
		
		UpdateHighScore();
		SaveHighScore();
		
		_hud.Visible = false;
		_gameOverScreen.Visible = true;
		_gameOverScreen.ShowScreen(_currentScore, _highScore);
	}
	
	
	private void RestartGame()
	{
		CleanupPipes();
		
		_player.Reset();
		_floor.Reset();
		_background.Reset();
		
		_currentState = GameState.Start;
		
		_gameOverScreen.Visible = false;
		_startScreen.Visible = true;
		_hud.Visible = false;
	}
	
	
	private void SpawnPipe()
	{
		if (_pipeScene == null)
			return;
		
		var pipe = _pipeScene.Instantiate<Node2D>();
		float randomY = (float)GD.RandRange(MinSpawnY, MaxSpawnY);
		pipe.Position = new Vector2(SpawnXPosition, randomY);
		
		AddChild(pipe);
	}
	
	
	private void CleanupPipes()
	{
		foreach (Node child in GetChildren())
		{
			if (child.Name.ToString().Contains("Pipe") || child is Pipe)
			{
				child.QueueFree();
			}
		}
	}
	
	
	private void StopAllPipes()
	{
		foreach (Node child in GetChildren())
		{
			if (child is Pipe pipe)
			{
				pipe.StopScrolling();
			}
		}
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
	}
	
	
	private void OnPlayerDied()
	{
		if (_deathSound.Stream != null)
		{
			_deathSound.Play();
			_deathSound.Seek(0.25f);
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
	}
	
	
	private void UpdateHighScore()
	{
		if (_currentScore > _highScore)
		{
			_highScore = _currentScore;
		}
	}
}
