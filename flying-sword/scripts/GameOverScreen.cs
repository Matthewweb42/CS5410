using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	private Label _scoreLabel;
	private Label _highScoreLabel;
	
	
	public override void _Ready()
	{
		// Try to get score labels if they exist
		_scoreLabel = GetNodeOrNull<Label>("ScoreLabel");
		_highScoreLabel = GetNodeOrNull<Label>("HighScoreLabel");
		GD.Print("GameOverScreen ready!");
	}
	
	
	public void ShowScreen(int score, int highScore)
	{
		if (_scoreLabel != null)
			_scoreLabel.Text = "Score: " + score;
		if (_highScoreLabel != null)
			_highScoreLabel.Text = "High Score: " + highScore;
		
		Visible = true;
	}
	
	
	public void HideScreen()
	{
		Visible = false;
	}
}
