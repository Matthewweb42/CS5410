using Godot;
using System;

public partial class Player : Area2D
{
	private const float FlapStrength = -350.0f;
	private const float GravityForce = 800.0f;
	private const float MaxFallSpeed = 500.0f;
	private const float MaxRotation = 0.75f;
	private const float MinRotation = -0.75f;
	private const float RotationSpeed = 5.0f;
	private const float FloorY = 920.0f;
	
	private float _velocityY = 0.0f;
	private bool _isAlive = true;
	private bool _hasGameStarted = false;
	private Vector2 _startPosition;
	
	private Sprite2D _sprite;
	private CollisionShape2D _collisionShape;
	private AudioStreamPlayer2D _flapSound;
	
	[Signal]
	public delegate void PlayerDiedEventHandler();
	
	[Signal]
	public delegate void PlayerScoredEventHandler();
	
	
	public override void _Ready()
	{
		_startPosition = Position;
		
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_flapSound = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
	}
	
	
	public override void _Process(double delta)
	{
		if (!_isAlive || !Input.IsActionJustPressed("flap"))
			return;
			
		Flap();
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		if (!_hasGameStarted)
			return;

		_velocityY += GravityForce * (float)delta;
		_velocityY = Mathf.Min(_velocityY, MaxFallSpeed);

		var newPosition = Position;
		newPosition.Y += _velocityY * (float)delta;
		Position = newPosition;

		UpdateRotation();
		CheckCeilingCollision();
		CheckFloorCollision();
	}
	
	
	private void Flap()
	{
		_velocityY = FlapStrength;

		if (_flapSound != null && _flapSound.Stream != null)
		{
			_flapSound.Play();
			_flapSound.Seek(0.25f);
		}
	}
	
	
	private void UpdateRotation()
	{
		float targetRotation = _velocityY * 0.002f;
		targetRotation = Mathf.Clamp(targetRotation, MinRotation, MaxRotation);
		Rotation = Mathf.Lerp(Rotation, targetRotation, RotationSpeed * (float)GetPhysicsProcessDeltaTime());
	}
	
	
	private void CheckCeilingCollision()
	{
		const float minY = 20.0f;

		if (Position.Y < minY)
		{
			var newPosition = Position;
			newPosition.Y = minY;
			Position = newPosition;
			_velocityY = 0.0f;
		}
	}
	
	
	private void CheckFloorCollision()
	{
		if (Position.Y >= FloorY)
		{
			// If player hits the ground while alive, they die
			if (_isAlive)
			{
				Die();
			}
			
			// Clamp position to floor
			var newPosition = Position;
			newPosition.Y = FloorY;
			Position = newPosition;
			_velocityY = 0.0f;
		}
	}
	
	
	public void Die()
	{
		if (!_isAlive)
			return;

		_isAlive = false;
		EmitSignal(SignalName.PlayerDied);
	}
	
	
	public void Reset()
	{
		Position = _startPosition;
		_velocityY = 0.0f;
		Rotation = 0.0f;
		_isAlive = true;
		_hasGameStarted = false;
	}
	
	
	public void StartPlaying()
	{
		_hasGameStarted = true;
		_isAlive = true;
	}
	
	
	private void OnAreaEntered(Area2D area)
	{
		string areaName = area.Name;

		if (areaName == "TopPipe" || areaName == "BottomPipe")
		{
			Die();
		}
		else if (areaName == "ScoreZone")
		{
			EmitSignal(SignalName.PlayerScored);
		}
	}
	
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Floor")
		{
			Die();
		}
	}
}
