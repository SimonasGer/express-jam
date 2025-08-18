using Godot;
using System;

public partial class MapButton : Button
{
	private WorldMap worldMap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		worldMap = GetNode<WorldMap>("/root/WorldMap");

		Pressed += OnButtonPressed;
	}
	private void OnButtonPressed()
	{
		switch (Text)
		{
			case "Dive":
				OnDive();
				break;

			case "Travel":
				OnTravel();
				break;

			default:
				GD.Print("Unknown button text: " + Text);
				break;
		}
	}

	private void OnDive()
	{
		var gameData = GetNode<GameData>("/root/GameData");

		gameData.TileData = worldMap.dict;
		gameData.PlayerGridPos = worldMap.playerPos;
		gameData.CoastGridPos = worldMap.coastPos;
		gameData.FishCount = worldMap.caughtFish;
		gameData.RevealedTiles = worldMap.revealedTiles;

		gameData.Save();

		GetTree().ChangeSceneToFile("res://scenes/surface.tscn");
	}

	private void OnTravel()
	{
		worldMap.playerPos = worldMap.gridPos;
		Vector2 worldPos = worldMap.map.MapToLocal(worldMap.gridPos);
		worldMap.player.Position = worldPos;
		worldMap.fow.EraseCell(worldMap.gridPos);
		worldMap.SelectTile(worldMap.gridPos);
		worldMap.revealedTiles.Add(worldMap.playerPos);
	}
}
