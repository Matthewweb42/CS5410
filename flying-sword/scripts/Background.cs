using Godot;
using System;

public partial class Background : Sprite2D
{
	// Stationary background - no movement
	private Vector2 _startPosition;
	
	
	public override void _Ready()
	{
		// Save the position from scene editor
		_startPosition = Position;
		
		// Make sure the sprite isn't centered so positioning works correctly
		Centered = false;
		
		GD.Print("Background initialized (stationary)");
	}
	
	
	// Empty methods kept for compatibility with GameManager
	public void StopScrolling()
	{
		// No-op - background is stationary
	}
	
	
	public void StartScrolling()
	{
		// No-op - background is stationary
	}
	
	
	public void Reset()
	{
		// Reset to starting position from scene
		Position = _startPosition;
		GD.Print("Background reset (stationary)");
	}
}
