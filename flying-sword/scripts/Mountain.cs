using Godot;
using System;

public partial class Mountain : Node2D
{
	// Mountain movement constants
	private const float ScrollSpeed = 150.0f;  // Same speed as floor scrolling
	
	// Mountain gap and size
	private const float GapSize = 200.0f;  // Vertical gap between top and bottom mountains
	private const float MinY = 200.0f;  // Minimum y position for gap center
	private const float MaxY = 500.0f;  // Maximum y position for gap center
	
	// References to child nodes
	private Sprite2D _topMountain;
	private Sprite2D _bottomMountain;
	private Area2D _scoreZone;
	private CollisionShape2D _topCollision;
	private CollisionShape2D _bottomCollision;
	
	// State
	private bool _hasScored = false;
	private bool _isScrolling = true;
	
	
	public override void _Ready()
	{
		// Get references to child nodes
		_topMountain = GetNode<Sprite2D>("TopMountain");
		_bottomMountain = GetNode<Sprite2D>("BottomMountain");
		_scoreZone = GetNode<Area2D>("ScoreZone");
		_topCollision = GetNode<CollisionShape2D>("TopMountain/CollisionShape2D");
		_bottomCollision = GetNode<CollisionShape2D>("BottomMountain/CollisionShape2D");
		
		// Note: Position is set by GameManager when spawning
		// The Y position passed to this mountain will be the center of the gap
		SetGapPosition(Position.Y);
		
		// Connect ScoreZone signal
		_scoreZone.AreaEntered += OnScoreZoneAreaEntered;
		
		GD.Print($"Mountain initialized at position {Position}");
	}
	
	
	public override void _Process(double delta)
	{
		// Move mountain to the left if scrolling is active
		if (_isScrolling)
		{
			Position = new Vector2(Position.X - ScrollSpeed * (float)delta, Position.Y);
			
			// Remove from memory when off screen
			if (IsOffScreen())
			{
				GD.Print("Mountain off screen, removing...");
				QueueFree();
			}
		}
	}
	
	
	private void SetGapPosition(float gapCenterY)
	{
		// Position top mountain above the gap
		_topMountain.Position = new Vector2(0, -GapSize / 2 - _topMountain.Texture.GetHeight() / 2);
		
		// Position bottom mountain below the gap
		_bottomMountain.Position = new Vector2(0, GapSize / 2 + _bottomMountain.Texture.GetHeight() / 2);
		
		// Position score zone in the middle of the gap
		_scoreZone.Position = new Vector2(0, 0);
	}
	
	
	private bool IsOffScreen()
	{
		// Check if mountain has moved past left edge of screen
		// Using -100 to give some buffer before cleanup
		return Position.X < -100;
	}
	
	
	public void StopScrolling()
	{
		_isScrolling = false;
	}
	
	
	public void StartScrolling()
	{
		_isScrolling = true;
	}
	
	
	private void OnScoreZoneAreaEntered(Area2D area)
	{
		// Check if the player entered the score zone
		if (!_hasScored && area.GetParent() is Player)
		{
			_hasScored = true;
			GD.Print("Player passed through mountain gap!");
			// Player will handle emitting the PlayerScored signal
		}
	}
}
