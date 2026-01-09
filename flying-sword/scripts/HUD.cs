using Godot;
using System;

public partial class HUD : CanvasLayer
{
	private Label _scoreLabel;
	
	
	public override void _Ready()
	{
		_scoreLabel = GetNodeOrNull<Label>("ScoreLabel");
		GD.Print("HUD ready!");
	}
	
	
	public void UpdateScore(int score)
	{
		if (_scoreLabel != null)
			_scoreLabel.Text = score.ToString();
	}
	
	
	public void ShowHud()
	{
		Visible = true;
	}
	
	
	public void HideHud()
	{
		Visible = false;
	}
}
