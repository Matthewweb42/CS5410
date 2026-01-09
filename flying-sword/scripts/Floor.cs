using Godot;
using System;

public partial class Floor : StaticBody2D
{
	// Floor scrolling constants
	private const float ScrollSpeed = 150.0f;  // Pixels per second (matches mountain speed)
	private const float FloorWidth = 576.0f;  // Width of floor sprite (matches screen width)
	private const float FloorYPosition = 924.0f;  // Y position at bottom of screen (1024 - 100 for floor height)

	// References
	private Sprite2D _sprite;

	// For infinite scrolling
	private float _scrollOffset = 0.0f;
	private bool _isScrolling = false;
	
	
	public override void _Ready()
	{
		// _Ready() initializes the floor when the scene loads

		// STEP 1: Get reference to the Sprite2D child node
		// This must match the name in floor.tscn
		_sprite = GetNode<Sprite2D>("Sprite2D");

		// STEP 2: Position the floor at the bottom of the screen
		// In Godot, Y increases downward, so higher Y = lower on screen
		// This places the floor near the bottom (1024px screen - 100px floor height)
		Position = new Vector2(0.0f, FloorYPosition);

		// STEP 3: Enable region rendering for infinite scrolling
		// Region rendering lets us display a portion of a larger texture
		// This is how we create the scrolling effect
		_sprite.RegionEnabled = true;
		_sprite.RegionRect = new Rect2(0, 0, FloorWidth, 112);  // Start showing first 576px width

		// Debug: Confirm initialization
		GD.Print("Floor initialized and ready!");
	}
	
	
	public override void _Process(double delta)
	{
		// _Process() creates the infinite scrolling effect every frame

		// STEP 1: Only scroll when the game is active
		// When game is paused or not started, floor stays still
		if (!_isScrolling)
			return;

		// STEP 2: Update scroll offset
		// Move the texture to the left by ScrollSpeed pixels per second
		// Cast delta to float for type compatibility
		_scrollOffset += ScrollSpeed * (float)delta;

		// STEP 3: Loop the texture when it scrolls past its width
		// Modulo (%) wraps the offset back to 0 when it reaches FloorWidth
		// This creates the infinite loop illusion
		// Example: If offset = 580 and width = 576, then 580 % 576 = 4 (wraps around)
		_scrollOffset = _scrollOffset % FloorWidth;

		// STEP 4: Update the sprite's region to show the scrolled portion
		// RegionRect defines which part of the texture is visible
		// X = _scrollOffset shifts the visible portion to the left
		// This makes it look like the floor is moving
		_sprite.RegionRect = new Rect2(_scrollOffset, 0, FloorWidth, 112);

		// Debug: Uncomment to see scroll offset (useful for testing loop behavior)
		// GD.Print($"Floor offset: {_scrollOffset:F1}");
	}
	
	
	public void StopScrolling()
	{
		// StopScrolling() is called when the player dies or game is paused
		// The GameManager calls this to freeze the floor in place

		_isScrolling = false;

		// Debug: Confirm scrolling stopped
		GD.Print("Floor scrolling stopped");
	}


	public void StartScrolling()
	{
		// StartScrolling() is called when the game starts
		// The GameManager calls this to begin the scrolling animation

		_isScrolling = true;

		// Debug: Confirm scrolling started
		GD.Print("Floor scrolling started");
	}


	public void Reset()
	{
		// Reset() restores the floor to its initial state
		// Called by GameManager when starting a new game

		// STEP 1: Reset scroll offset to beginning
		// This shows the start of the texture again
		_scrollOffset = 0.0f;

		// STEP 2: Reset the region rect to show the first portion
		// Back to the starting view of the texture
		_sprite.RegionRect = new Rect2(0, 0, FloorWidth, 112);

		// STEP 3: Stop scrolling
		// Floor should be stationary until game starts
		_isScrolling = false;

		// Debug: Confirm reset
		GD.Print("Floor reset to initial state");
	}
}
