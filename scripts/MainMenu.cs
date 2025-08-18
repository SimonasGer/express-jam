using Godot;
using System;

public partial class MainMenu : Control
{
	private Button newGame, loadGame, quit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		newGame = GetNode<Button>("New");
		loadGame = GetNode<Button>("Load");
		quit = GetNode<Button>("Quit");

		newGame.Pressed += OnStart;
		loadGame.Pressed += OnLoad;
		quit.Pressed += OnQuit;

	}

	private void OnStart()
	{
		GetTree().ChangeSceneToFile("res://scenes/world_map.tscn");
	}

	private void OnLoad()
	{
		var gameData = GetNode<GameData>("/root/GameData");
		gameData.Load();
		GetTree().ChangeSceneToFile("res://scenes/world_map.tscn");
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
