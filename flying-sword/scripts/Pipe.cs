using Godot;
using System;

public partial class Pipe : Node2D
{
	// Movement constants
	private const float ScrollSpeed = 150.0f;  // Pixels per second (matches floor/background speed)
	private const float DeletePosition = -200.0f;  // Delete pipe when it goes this far off-screen
	
	// State
	private bool _isScrolling = true;
	
	
	public override void _Ready()
	{
		// All pipe visuals and collision shapes are set up in Godot editor
		// The scene structure should be:
		//   Pipe (Node2D) - root
		//   ├── TopPipeSprite (Sprite2D)
		//   ├── TopPipe (Area2D with CollisionShape2D)
		//   ├── BottomPipeSprite (Sprite2D)
		//   ├── BottomPipe (Area2D with CollisionShape2D)
		//   └── ScoreZone (Area2D with CollisionShape2D)
		
		GD.Print($"Pipe ready at position: {Position}");
	}
	
	
	public override void _Process(double delta)
	{
		// Handle pipe scrolling and cleanup
		
		if (!_isScrolling)
			return;  // Don't move if scrolling is paused
		
		// Move pipe left (scrolling effect)
		Position += new Vector2(-ScrollSpeed * (float)delta, 0);
		
		// Delete pipe when it goes off-screen to the left
		if (Position.X < DeletePosition)
		{
			QueueFree();  // Remove from scene
			GD.Print("Pipe deleted (off-screen)");
		}
	}
	
	
	public void StopScrolling()
	{
		// StopScrolling() is called when the player dies or game is paused
		// This freezes the pipe in place
		_isScrolling = false;
	}
	
	
	public void StartScrolling()
	{
		// StartScrolling() is called when the game starts or resumes
		// This enables pipe movement
		_isScrolling = true;
	}
}
