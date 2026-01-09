using Godot;
using System;

public partial class StartScreen : CanvasLayer
{
	public override void _Ready()
	{
		GD.Print("StartScreen ready!");
	}
	
	
	public void ShowScreen()
	{
		Visible = true;
	}
	
	
	public void HideScreen()
	{
		Visible = false;
	}
}
