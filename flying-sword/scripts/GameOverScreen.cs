using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	private Label _scoreLabel;
	private Label _highScoreLabel;
	
	
	public override void _Ready()
	{
		// Try to get score labels - check multiple paths
		_scoreLabel = GetNodeOrNull<Label>("CenterContainer/VBoxContainer/ScoreLabel");
		_highScoreLabel = GetNodeOrNull<Label>("CenterContainer/VBoxContainer/HighScoreLabel");
		
		if (_scoreLabel == null)
			GD.PrintErr("ScoreLabel not found in GameOverScreen!");
		if (_highScoreLabel == null)
			GD.PrintErr("HighScoreLabel not found in GameOverScreen!");
		
		GD.Print("GameOverScreen ready!");
	}
	
	
	public void ShowScreen(int score, int highScore)
	{
		GD.Print($"ShowScreen called with score: {score}, highScore: {highScore}");
		
		if (_scoreLabel != null)
		{
			_scoreLabel.Text = $"Score: {score}";
			GD.Print($"Set score label to: {_scoreLabel.Text}");
		}
		else
		{
			GD.PrintErr("Cannot update score - label is null");
		}
		
		if (_highScoreLabel != null)
		{
			_highScoreLabel.Text = $"High Score: {highScore}";
			GD.Print($"Set high score label to: {_highScoreLabel.Text}");
		}
		else
		{
			GD.PrintErr("Cannot update high score - label is null");
		}
		
		Visible = true;
	}
	
	
	public void HideScreen()
	{
		Visible = false;
	}
}
