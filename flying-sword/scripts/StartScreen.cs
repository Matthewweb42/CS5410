using Godot;
using System;

public partial class StartScreen : CanvasLayer
{
	private Label _titleLabel;
	private Label _instructionLabel;
	
	
	public override void _Ready()
	{
		// TODO: Initialize start screen
		// - Get references to labels using GetNode<Label>()
		// - Set up label fonts/sizes if needed (optional)
	}
	
	
	public void ShowScreen()
	{
		// TODO: Make start screen visible
		// - Set Visible = true
	}
	
	
	public void HideScreen()
	{
		// TODO: Hide start screen
		// - Set Visible = false
	}
}
