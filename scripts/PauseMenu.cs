using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	private Button resumeButton;
	private Button quitButton;
	private Panel panel;

	public override void _Ready()
	{
		resumeButton = GetNode<Button>("Panel/Resume");
		quitButton = GetNode<Button>("Panel/Quit");
		panel = GetNode<Panel>("Panel");

		resumeButton.Pressed += OnResume;
		quitButton.Pressed += OnQuit;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			TogglePause();
		}
	}

	private void TogglePause()
	{
		bool paused = !GetTree().Paused;
		GetTree().Paused = paused;
		panel.Visible = paused;
	}

	private void OnResume()
	{
		GetTree().Paused = false;
		panel.Visible = false;
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
