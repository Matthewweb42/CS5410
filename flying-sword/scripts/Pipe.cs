using Godot;
using System;

public partial class Pipe : Node2D
{
	// Movement
	private const float ScrollSpeed = 150.0f;
	
	// Pipe configuration
	[Export] public float GapSize = 200.0f;
	[Export] public Texture2D PipeTexture;
	[Export] public int PipeColorColumn = 0;  // Which pipe color to use (0-7 from your tileset)
	[Export] public int MiddleTileCount = 16;  // How many middle sections to stack
	
	// Texture regions for different pipe parts (adjust these based on your texture)
	private const int TileWidth = 32;  // Width of one pipe
	private const int TopCapYOffset = 48;  // Y position where top cap starts in texture
	private const int TopCapHeight = 16;  // Height of top cap
	private const int MiddleHeight = 16;  // Height of one middle section
	private const int BottomCapHeight = 16;  // Height of bottom cap
	
	// Child node references
	private Node2D _topPipeContainer;
	private Node2D _bottomPipeContainer;
	private Area2D _topPipeCollision;
	private Area2D _bottomPipeCollision;
	private Area2D _scoreZone;
	
	// State
	private bool _hasScored = false;
	private bool _isScrolling = true;
	
	
	public override void _Ready()
	{
		// Load texture if not assigned
		if (PipeTexture == null)
		{
			PipeTexture = GD.Load<Texture2D>("res://assets/sprites/PipeStyle2.png");
		}
		
		// Create containers
		_topPipeContainer = new Node2D();
		_topPipeContainer.Name = "TopPipeVisuals";
		AddChild(_topPipeContainer);
		
		_bottomPipeContainer = new Node2D();
		_bottomPipeContainer.Name = "BottomPipeVisuals";
		AddChild(_bottomPipeContainer);
		
		// Create collision areas
		_topPipeCollision = CreateCollisionArea("TopPipe");
		_bottomPipeCollision = CreateCollisionArea("BottomPipe");
		_scoreZone = CreateCollisionArea("ScoreZone");
		
		// Build the pipes
		BuildPipes();
		
		// Connect score zone
		_scoreZone.AreaEntered += OnScoreZoneEntered;
		
		GD.Print($"Pipe initialized at position {GlobalPosition}");
	}
	
	
	private Area2D CreateCollisionArea(string name)
	{
		var area = new Area2D();
		area.Name = name;
		AddChild(area);
		
		var collision = new CollisionShape2D();
		var shape = new RectangleShape2D();
		collision.Shape = shape;
		area.AddChild(collision);
		
		return area;
	}
	
	
	private void BuildPipes()
	{
		if (PipeTexture == null)
		{
			GD.PrintErr("Pipe texture not loaded!");
			return;
		}
		
		// Calculate X offset for selected pipe color
		int xOffset = PipeColorColumn * TileWidth;
		
		// Build TOP pipe (hangs down from above)
		BuildSinglePipe(_topPipeContainer, xOffset, false);
		
		// Build BOTTOM pipe (extends up from below)
		BuildSinglePipe(_bottomPipeContainer, xOffset, true);
		
		// Setup collisions (accounting for 2x scale)
		float totalPipeHeight = (TopCapHeight + (MiddleTileCount * MiddleHeight) + BottomCapHeight) * 2;
		SetupCollisions(TileWidth * 2, totalPipeHeight);
	}
	
	
	private void BuildSinglePipe(Node2D container, int xOffset, bool isTopPipe)
	{
		float currentY = 0;
		
		if (isTopPipe)
		{
			// Top pipe extends upward from gap edge
			currentY = -(GapSize / 2);
			
			// Top cap first (top end pointing up)
			var topCap = CreatePipeSprite(xOffset, TopCapYOffset, TileWidth, TopCapHeight);
			topCap.Position = new Vector2(0, currentY - TopCapHeight);
			topCap.FlipV = true;  // Flip to point upward
			container.AddChild(topCap);
			
			// Middle sections stacking upward
			for (int i = 0; i < MiddleTileCount; i++)
			{
				var middle = CreatePipeSprite(xOffset, TopCapHeight, TileWidth, MiddleHeight);
				middle.Position = new Vector2(0, currentY - (TopCapHeight * 2) - (i + 1) * MiddleHeight * 2);
				container.AddChild(middle);
			}
			
			// Bottom cap last (furthest from gap)
			var bottomCap = CreatePipeSprite(xOffset, TopCapHeight + MiddleHeight, TileWidth, BottomCapHeight);
			bottomCap.Position = new Vector2(0, currentY - (TopCapHeight * 2) - (MiddleTileCount * MiddleHeight * 2) - BottomCapHeight);
			bottomCap.FlipV = true;
			container.AddChild(bottomCap);
		}
		else
		{
			// Bottom pipe extends downward from gap edge
			currentY = GapSize / 2;
			
			// Top cap first (closest to gap, pointing down)
			var topCap = CreatePipeSprite(xOffset, TopCapYOffset, TileWidth, TopCapHeight);
			topCap.Position = new Vector2(0, currentY + TopCapHeight);
			container.AddChild(topCap);
			
			// Middle sections stacking downward
			for (int i = 0; i < MiddleTileCount; i++)
			{
				var middle = CreatePipeSprite(xOffset, TopCapHeight, TileWidth, MiddleHeight);
				middle.Position = new Vector2(0, currentY + (TopCapHeight * 2) + (i + 1) * MiddleHeight * 2);
				container.AddChild(middle);
			}
			
			// Bottom cap last (furthest from gap)
			var bottomCap = CreatePipeSprite(xOffset, TopCapHeight + MiddleHeight, TileWidth, BottomCapHeight);
			bottomCap.Position = new Vector2(0, currentY + (TopCapHeight * 2) + (MiddleTileCount * MiddleHeight * 2) + BottomCapHeight);
			container.AddChild(bottomCap);
		}
	}
	
	
	private Sprite2D CreatePipeSprite(int xOffset, int yOffset, int width, int height)
	{
		var sprite = new Sprite2D();
		sprite.Texture = PipeTexture;
		sprite.RegionEnabled = true;
		sprite.RegionRect = new Rect2(xOffset, yOffset, width, height);
		sprite.Centered = true;
		sprite.Scale = new Vector2(2, 2);  // Scale pipes to 2x size
		return sprite;
	}
	
	
	private void SetupCollisions(float width, float height)
	{
		// Top pipe collision
		var topShape = _topPipeCollision.GetChild<CollisionShape2D>(0).Shape as RectangleShape2D;
		topShape.Size = new Vector2(width, height);
		_topPipeCollision.Position = new Vector2(0, -(GapSize / 2) - (height / 2));
		
		// Bottom pipe collision
		var bottomShape = _bottomPipeCollision.GetChild<CollisionShape2D>(0).Shape as RectangleShape2D;
		bottomShape.Size = new Vector2(width, height);
		_bottomPipeCollision.Position = new Vector2(0, (GapSize / 2) + (height / 2));
		
		// Score zone collision
		var scoreShape = _scoreZone.GetChild<CollisionShape2D>(0).Shape as RectangleShape2D;
		scoreShape.Size = new Vector2(width, GapSize);
		_scoreZone.Position = Vector2.Zero;
	}
	
	
	public override void _Process(double delta)
	{
		if (_isScrolling)
		{
			Position += new Vector2(-ScrollSpeed * (float)delta, 0);
			
			if (Position.X < -200)
			{
				QueueFree();
			}
		}
	}
	
	
	public void StopScrolling()
	{
		_isScrolling = false;
	}
	
	
	public void StartScrolling()
	{
		_isScrolling = true;
	}
	
	
	private void OnScoreZoneEntered(Area2D area)
	{
		if (!_hasScored && area.GetParent() is Player)
		{
			_hasScored = true;
			GD.Print("Player scored!");
		}
	}
}
