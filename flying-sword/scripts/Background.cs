using Godot;
using System;

public partial class Background : Sprite2D
{
	// Background scrolling constants
	private const float ScrollSpeed = 5.0f;  // Much slower than floor (150) for parallax effect
	private const float StartY = 400.0f;  // Starting Y position (negative = higher up)
	
	// For infinite scrolling
	private float _scrollOffset = 0.0f;
	private bool _isScrolling = false;
	private float _textureWidth;
	
	
	public override void _Ready()
	{
		// Get the width of the background texture for seamless looping
		if (Texture != null)
		{
			_textureWidth = Texture.GetWidth();
		}
		else
		{
			GD.PrintErr("Background has no texture assigned!");
			_textureWidth = 1024.0f; // Default fallback
		}
		
		// Make sure the sprite isn't centered so positioning works correctly
		Centered = false;
		
		// Set initial position - start at 0,StartY so it's visible from the beginning
		Position = new Vector2(0, StartY);
		
		GD.Print("Background initialized and ready!");
	}
	
	
	public override void _Process(double delta)
	{
		// Only scroll when the game is active
		if (!_isScrolling)
			return;
		
		// Move the background left slowly
		_scrollOffset += ScrollSpeed * (float)delta;
		
		// When we've scrolled one full texture width, reset back
		// This creates seamless infinite scrolling
		if (_scrollOffset >= _textureWidth)
		{
			_scrollOffset -= _textureWidth;
		}
		
		// Apply position (negative X moves left)
		Position = new Vector2(-_scrollOffset, StartY);
	}
	
	
	public void StopScrolling()
	{
		_isScrolling = false;
		GD.Print("Background scrolling stopped");
	}
	
	
	public void StartScrolling()
	{
		_isScrolling = true;
		GD.Print("Background scrolling started");
	}
	
	
	public void Reset()
	{
		_scrollOffset = 0.0f;
		Position = new Vector2(0, StartY);
		_isScrolling = false;
		GD.Print("Background reset to initial state");
	}
}
