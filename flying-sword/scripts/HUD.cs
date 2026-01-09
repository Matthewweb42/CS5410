using Godot;
using System;

public partial class HUD : CanvasLayer
{
	private Label _scoreLabel;
	
	
	public override void _Ready()
	{
		// TODO: Initialize HUD
		// - Get reference to score label using GetNode<Label>()
		// - Set up label font/size (optional)
		// - Set initial score to "0"
	}
	
	
	public void UpdateScore(int score)
	{
		// TODO: Update score display
		// - Set _scoreLabel.Text to score.ToString()
	}
	
	
	public void ShowHud()
	{
		// TODO: Show HUD during gameplay
		// - Set Visible = true
	}
	
	
	public void HideHud()
	{
		// TODO: Hide HUD when not playing
		// - Set Visible = false
	}
}
