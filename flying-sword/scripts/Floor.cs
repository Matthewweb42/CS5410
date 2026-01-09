using Godot;
using System;

public partial class Floor : StaticBody2D
{
	// Floor scrolling constants
	private const float ScrollSpeed = 150.0f;  // Pixels per second (matches mountain speed)
	private const float TileWidth = 16.0f;     // Width of one tile in your tileset (16x16 tiles)

	// References
	private TileMapLayer _tileMapLayer;

	// For infinite scrolling
	private float _scrollOffset = 0.0f;
	private bool _isScrolling = false;


	public override void _Ready()
	{
		// Get reference to the TileMapLayer child node
		_tileMapLayer = GetNode<TileMapLayer>("TileMapLayer");

		GD.Print("Floor initialized and ready!");
	}


	public override void _Process(double delta)
	{
		// Only scroll when the game is active
		if (!_isScrolling)
			return;

		// Move the tilemap left
		_scrollOffset += ScrollSpeed * (float)delta;

		// When we've scrolled one full tile width, reset back
		// This creates seamless infinite scrolling
		if (_scrollOffset >= TileWidth)
		{
			_scrollOffset -= TileWidth;
		}

		// Apply position (negative X moves left)
		_tileMapLayer.Position = new Vector2(-_scrollOffset, _tileMapLayer.Position.Y);
	}


	public void StopScrolling()
	{
		// StopScrolling() is called when the player dies or game is paused
		_isScrolling = false;
		GD.Print("Floor scrolling stopped");
	}


	public void StartScrolling()
	{
		// StartScrolling() is called when the game starts
		_isScrolling = true;
		GD.Print("Floor scrolling started");
	}


	public void Reset()
	{
		// Reset() restores the floor to its initial state
		_scrollOffset = 0.0f;
		_tileMapLayer.Position = new Vector2(0, _tileMapLayer.Position.Y);
		_isScrolling = false;
		GD.Print("Floor reset to initial state");
	}
}
