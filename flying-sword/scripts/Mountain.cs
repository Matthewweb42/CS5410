using Godot;
using System;

public partial class Mountain : Node2D
{
	// Mountain movement constants
	private const float ScrollSpeed = 0.0f;  // TODO: Set speed at which mountains move left
	
	// Mountain gap and size
	private const float GapSize = 0.0f;  // TODO: Set vertical gap between top and bottom mountains
	private const float MinY = 0.0f;  // TODO: Set minimum y position for gap center
	private const float MaxY = 0.0f;  // TODO: Set maximum y position for gap center
	
	// References to child nodes
	private Area2D _topMountain;
	private Area2D _bottomMountain;
	private Area2D _scoreZone;
	
	// State
	private bool _hasScored = false;
	
	
	public override void _Ready()
	{
		// TODO: Initialize the mountain pair
		// - Get references to child nodes using GetNode<Area2D>()
		// - Set random y position for the gap
		// - Call SetGapPosition() with random y position
		// - Set up collision layers/masks
		// - Connect ScoreZone AreaEntered signal to OnScoreZoneAreaEntered
	}
	
	
	public override void _Process(double delta)
	{
		// TODO: Move mountain to the left
		// - Update Position.X based on ScrollSpeed and delta (cast delta to float)
		// - Create new Vector2 for position: Position = new Vector2(x, Position.Y)
		// - Check if mountain is off screen using IsOffScreen()
		// - If off screen, call QueueFree() to remove from memory
	}
	
	
	private void SetGapPosition(float yPos)
	{
		// TODO: Position the mountains with gap at yPos
		// - Set top mountain position (above yPos)
		// - Set bottom mountain position (below yPos)
		// - Set score zone position (at yPos)
		// - Use _topMountain.Position = new Vector2(x, y)
	}
	
	
	private bool IsOffScreen()
	{
		// TODO: Check if mountain has moved past left edge of screen
		// - Return true if Position.X is less than a threshold (e.g., -100)
		// - This helps with cleanup
		return false;
	}
	
	
	private void OnScoreZoneAreaEntered(Area2D area)
	{
		// TODO: Handle player entering score zone
		// - Check if it's the player using area.GetParent().Name
		// - If not already scored (_hasScored == false), set _hasScored = true
		// - Player will emit their own PlayerScored signal when they detect the score zone
	}
}
