using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	private Label _gameOverLabel;
	private Label _scoreLabel;
	private Label _highScoreLabel;
	private Label _restartLabel;
	
	
	public override void _Ready()
	{
		// TODO: Initialize game over screen
		// - Get references to labels using GetNode<Label>()
		// - Set up label fonts/sizes if needed (optional)
		// - Hide screen initially using Visible = false
	}
	
	
	public void ShowScreen(int score, int highScore)
	{
		// TODO: Display game over screen with scores
		// - Update _scoreLabel.Text with "Score: " + score
		// - Update _highScoreLabel.Text with "High Score: " + highScore
		// - Set Visible = true
	}
	
	
	public void HideScreen()
	{
		// TODO: Hide game over screen
		// - Set Visible = false
	}
}
