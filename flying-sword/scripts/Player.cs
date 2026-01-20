using Godot;
using System;

public partial class Player : Area2D
{
	// Sword movement and physics constants
	private const float FlapStrength = -350.0f;  // Negative = upward velocity when clicking
	private const float Gravity = 800.0f;  // Downward acceleration (pulls sword down)
	private const float MaxFallSpeed = 500.0f;  // Prevent falling too fast
	private const float MaxRotation = 0.75f;  // Max rotation angle in radians (45 degrees)
	private const float MinRotation = -0.75f;  // Min rotation angle in radians (-45 degrees)
	private const float RotationSpeed = 5.0f;  // How fast the sword rotates
	
	// Player state
	private float _velocityY = 0.0f;
	private bool _isAlive = true;
	private Vector2 _startPosition;  // Store initial position from scene
	
	// References to child nodes
	private Sprite2D _sprite;  // Using simple Sprite2D instead of AnimatedSprite2D for now
	private CollisionShape2D _collisionShape;
	private AudioStreamPlayer2D _flapSound;
	
	// Signals to communicate with game manager
	[Signal]
	public delegate void PlayerDiedEventHandler();
	
	[Signal]
	public delegate void PlayerScoredEventHandler();
	
	
	public override void _Ready()
	{
		// Save starting position from scene
		_startPosition = Position;
		
		// STEP 1: Get references to child nodes
		// GetNode<T>() finds child nodes by their name in the scene tree
		// The names must match exactly what's in player.tscn
		// Note: Change "Sprite2D" to "AnimatedSprite2D" if you switch to animations later
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_flapSound = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		// STEP 2: Connect collision signals
		// In C#, we use += to connect signals to methods
		// AreaEntered fires when another Area2D overlaps this one (mountains, score zones)
		AreaEntered += OnAreaEntered;

		// BodyEntered fires when a StaticBody2D or RigidBody2D touches this Area2D (floor)
		BodyEntered += OnBodyEntered;

		// STEP 3: Optional - Set up sprite animation
		// If you switch to AnimatedSprite2D later, you can play animations here
		// For now, we're using a simple static Sprite2D
		// _sprite.Play("idle");  // Only works with AnimatedSprite2D

		// STEP 4: Print debug message to confirm initialization
		GD.Print("Player initialized and ready!");
	}
	
	
	public override void _Process(double delta)
	{
		// _Process() runs every frame (typically 60 times per second)
		// This is where we check for player input

		// Check if the player is still alive before accepting input
		// No point in flapping if we're dead!
		if (!_isAlive)
			return; // Exit early if dead

		// Input.IsActionJustPressed() checks if an action was JUST pressed THIS frame
		// "flap" is defined in project.godot and includes:
		//   - Left mouse button click
		//   - Spacebar press
		// The "Just" part means it only returns true for ONE frame when pressed
		// (This prevents holding the button from spamming flaps)
		if (Input.IsActionJustPressed("flap"))
		{
			Flap(); // Call the Flap method (we'll implement this next!)

			// Debug: Print when flap is triggered
			GD.Print("Flap input detected!");
		}
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		// _PhysicsProcess() runs at a FIXED rate (typically 60 times per second)
		// Unlike _Process(), this is frame-rate independent
		// Perfect for physics calculations like gravity and movement!

		// STEP 1: Apply gravity
		// Gravity constantly pulls the player downward
		// We ADD gravity to velocity every frame (acceleration!)
		// Cast delta to float because it's a double, but our velocity is float
		_velocityY += Gravity * (float)delta;

		// STEP 2: Clamp velocity to prevent falling too fast
		// Mathf.Min returns the SMALLER of two values
		// This caps falling speed at MaxFallSpeed
		// Note: Only clamps downward velocity (positive), not upward (negative)
		_velocityY = Mathf.Min(_velocityY, MaxFallSpeed);

		// STEP 3: Update position based on velocity
		// Position is READ-ONLY in C#, so we must create a new Vector2
		// velocity * delta converts "pixels per second" to "pixels per frame"
		var newPosition = Position;
		newPosition.Y += _velocityY * (float)delta;
		Position = newPosition;

		// STEP 4: Update rotation based on velocity
		// Makes the sword tilt up when going up, down when falling
		UpdateRotation();

		// STEP 5: Prevent flying off the top of the screen
		CheckCeilingCollision();

		// Debug: Show current state (comment out after testing - it's spammy!)
		// GD.Print($"Vel: {_velocityY:F1}, Pos: {Position.Y:F1}");
	}
	
	
	private void Flap()
	{
		// Flap() is called when the player presses the flap button
		// This gives the sword an upward boost

		// Set vertical velocity to FlapStrength (a negative value = upward movement)
		// FlapStrength is -400.0f, so this makes the sword jump UP
		// This REPLACES the current velocity (not adds to it)
		// Why? If falling at -500 and we just added -400, we'd get -900 (super fast!)
		// By setting it directly, every flap feels consistent
		_velocityY = FlapStrength;

		// Play the flap sound effect (if audio file is assigned in Godot editor)
		// If no audio is assigned yet, this won't crash - it just does nothing
		// You'll add the actual sound file later in the Godot editor
		if (_flapSound != null && _flapSound.Stream != null)
		{
			_flapSound.Play();
		}

		// Optional: Trigger flap animation
		// Only works with AnimatedSprite2D, not regular Sprite2D
		// _sprite.Play("flap");  // Only works with AnimatedSprite2D

		// Debug: Confirm flap was executed
		GD.Print($"Flap! Velocity set to: {_velocityY}");
	}
	
	
	private void UpdateRotation()
	{
		// UpdateRotation() makes the sword tilt based on movement direction
		// This creates a more realistic flying effect - sword points up when going up, down when falling

		// STEP 1: Calculate target rotation from velocity
		// We need to map velocity (-400 to +300) to rotation range (-0.75 to +0.75 radians)
		// The magic number here is a scale factor: how much velocity affects rotation
		// 0.002f means: for every 100 pixels/sec of velocity, rotate ~0.2 radians (~11 degrees)
		// When velocity is negative (going up), rotation becomes negative (tilts up)
		// When velocity is positive (falling), rotation becomes positive (tilts down)
		float targetRotation = _velocityY * 0.002f;

		// STEP 2: Clamp the target rotation within our min/max bounds
		// Without clamping, the sword could rotate completely upside down at high speeds!
		// Mathf.Clamp(value, min, max) restricts the value to stay within the range
		// This keeps rotation between -0.75 and +0.75 radians (about -43° to +43°)
		targetRotation = Mathf.Clamp(targetRotation, MinRotation, MaxRotation);

		// STEP 3: Smoothly interpolate to target rotation
		// Mathf.Lerp(start, end, t) does linear interpolation: start + (end - start) * t
		// - Rotation is current rotation (where we are now)
		// - targetRotation is where we want to be
		// - RotationSpeed * delta controls how fast we get there
		// Instead of snapping instantly, this creates smooth rotation transitions
		// RotationSpeed = 5.0f means we move 5% of the remaining distance each second
		// Higher values = faster rotation, lower values = slower/smoother rotation
		Rotation = Mathf.Lerp(Rotation, targetRotation, RotationSpeed * (float)GetPhysicsProcessDeltaTime());

		// Debug: Uncomment to see rotation values (helps tune the scale factor and speed)
		// GD.Print($"Vel: {_velocityY:F1}, Target: {targetRotation:F3}, Current: {Rotation:F3}");
	}
	
	
	private void CheckCeilingCollision()
	{
		// CheckCeilingCollision() prevents the player from flying off the top of the screen
		// Without this, the player could flap infinitely upward and disappear!

		// Define the minimum Y position (top of screen boundary)
		// In Godot, Y=0 is the top of the screen, so we add a small buffer
		// This prevents the sword from going completely off-screen
		const float minY = 20.0f;

		// Check if the player has gone above the ceiling
		if (Position.Y < minY)
		{
			// Clamp the position to the minimum Y value
			// This "snaps" the player back to the ceiling boundary
			var newPosition = Position;
			newPosition.Y = minY;
			Position = newPosition;

			// Stop upward velocity when hitting the ceiling
			// This prevents the player from "fighting" against the ceiling
			// Without this, velocity would keep getting more negative while stuck at ceiling
			_velocityY = 0.0f;

			// Debug: Uncomment to see when ceiling is hit
			// GD.Print("Hit ceiling!");
		}
	}
	
	
	public void Die()
	{
		// Die() handles everything that happens when the player hits an obstacle
		// This is called from OnAreaEntered() (mountains) or OnBodyEntered() (floor)

		// STEP 1: Prevent dying twice
		// If already dead, ignore subsequent collisions
		// Without this check, hitting multiple obstacles would emit the signal multiple times!
		if (!_isAlive)
			return; // Already dead, do nothing

		// STEP 2: Mark player as dead
		// This stops input processing in _Process()
		// The player can no longer flap once dead
		_isAlive = false;

		// STEP 3: Emit the PlayerDied signal
		// This tells the GameManager to transition to the Game Over state
		// Signals are how child nodes communicate UP to parent nodes
		// EmitSignal uses the SignalName enum to reference our signal safely
		EmitSignal(SignalName.PlayerDied);

		// STEP 4: Optional - Play death animation
		// Only works with AnimatedSprite2D, not regular Sprite2D
		// _sprite.Play("death");  // Only works with AnimatedSprite2D

		// STEP 5: Optional - Stop collision detection
		// You might want to disable collision so the player falls through everything
		// Uncomment if you want the player to keep falling after death:
		// _collisionShape.SetDeferred("disabled", true);

		// Note: We DON'T play a death sound here because:
		// The GameManager has the death sound node and will play it when it receives the signal
		// This follows the architecture: player reports death, manager handles audio/effects

		// Debug: Confirm death was triggered
		GD.Print("Player died!");
	}
	
	
	public void Reset()
	{
		// Reset() restores the player to the initial starting state
		// This is called by the GameManager when starting a new game
		// After game over, this prepares the player for another attempt

		// STEP 1: Reset position to starting location
		// Use the position set in the scene editor
		Position = _startPosition;

		// STEP 2: Reset physics state
		// Clear any existing velocity from the previous game
		// Player starts motionless, not falling or flying
		_velocityY = 0.0f;

		// STEP 3: Reset rotation
		// Player starts perfectly horizontal (0 radians = 0 degrees)
		// No upward or downward tilt at the start
		Rotation = 0.0f;

		// STEP 4: Mark player as alive
		// This re-enables input processing in _Process()
		// Player can now respond to flap commands again
		_isAlive = true;

		// STEP 5: Optional - Re-enable collision if it was disabled on death
		// Uncomment if you disabled collision in Die()
		// _collisionShape.SetDeferred("disabled", false);

		// STEP 6: Optional - Reset to idle animation
		// Only works with AnimatedSprite2D, not regular Sprite2D
		// _sprite.Play("idle");  // Only works with AnimatedSprite2D

		// Debug: Confirm reset was executed
		GD.Print("Player reset to starting state!");
	}
	
	
	private void OnAreaEntered(Area2D area)
	{
		// OnAreaEntered() is called when the player overlaps with another Area2D
		// This handles collisions with pipes (death) and score zones (points)
		// This signal was connected in _Ready() with: AreaEntered += OnAreaEntered

		// STEP 1: Identify what we collided with
		// We need to check the NAME of the area to determine what it is
		// In pipe.tscn, we have three Area2D children:
		//   - "TopPipe" (kills player)
		//   - "BottomPipe" (kills player)
		//   - "ScoreZone" (gives points)

		// Get the name of the Area2D that was hit
		string areaName = area.Name;

		// STEP 2: Handle pipe collisions (death)
		// If the area is named "TopPipe" or "BottomPipe", the player dies
		if (areaName == "TopPipe" || areaName == "BottomPipe")
		{
			// Call Die() to handle death sequence
			Die();

			// Debug: Show which pipe was hit
			GD.Print($"Hit {areaName}!");
		}

		// STEP 3: Handle score zone collision (scoring)
		// If the area is named "ScoreZone", the player scored a point!
		else if (areaName == "ScoreZone")
		{
			// Emit the PlayerScored signal
			// This tells the GameManager to increment the score
			// The signal flows UP: Player → GameManager → HUD
			EmitSignal(SignalName.PlayerScored);

			// Debug: Confirm score was registered
			GD.Print("Scored a point!");
		}

		// STEP 4: Handle unexpected collisions (debugging)
		else
		{
			// If we hit something unexpected, log it for debugging
			// This helps catch naming mistakes in scene files
			GD.Print($"Unknown area entered: {areaName}");
		}
	}
	
	
	private void OnBodyEntered(Node2D body)
	{
		// OnBodyEntered() is called when the player collides with a StaticBody2D or RigidBody2D
		// This handles collision with the floor (the only body in our game)
		// This signal was connected in _Ready() with: BodyEntered += OnBodyEntered

		// STEP 1: Identify what body we hit
		// In our game, the only StaticBody2D is the Floor
		// We can check the name to be safe, but any body collision means death
		string bodyName = body.Name;

		// STEP 2: Handle floor collision (death)
		// If the player touches the floor, they die
		if (bodyName == "Floor")
		{
			// Call Die() to handle death sequence
			Die();

			// Debug: Confirm floor collision
			GD.Print("Hit the floor!");
		}
		else
		{
			// If we hit an unexpected body, log it for debugging
			// In a more complex game, you might have platforms, enemies, etc.
			GD.Print($"Unknown body entered: {bodyName}");

			// Still die on any body collision to be safe
			Die();
		}
	}
}
