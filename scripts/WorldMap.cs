using Godot;
using System;
using System.Collections.Generic;

public partial class WorldMap : Node2D
{
	public TileMapLayer fow, map;
	public int caughtFish = 0;
	private Vector2 fishRate = new(50, 150); //will adjust numbers later
	private Vector2 bubbleRate = new(50, 150);
	private Vector2 bombRate = new(50, 150);
	public Sprite2D player;
	private Button button;
	private Label info;
	public Dictionary<Vector2I, Vector3I> dict = new(36);
	public List<Vector2I> revealedTiles = [];
	public Vector2I playerPos;
	public Vector2I coastPos;
	public Vector2I gridPos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetNode<TileMapLayer>("Map");
		fow = GetNode<TileMapLayer>("Fow");
		player = GetNode<Sprite2D>("Player");
		button = GetNode<Button>("CanvasLayer/Panel/Button");
		info = GetNode<Label>("CanvasLayer/Panel/Label");
		
		var gameData = GetNode<GameData>("/root/GameData");

		if (gameData.TileData.Count > 0)
		{
			LoadData();
			return;
		}
		PopulateData();
	}

	private void LoadData()
	{
		var gameData = GetNode<GameData>("/root/GameData");

		dict = gameData.TileData;
		playerPos = gameData.PlayerGridPos;
		coastPos = gameData.CoastGridPos;
		caughtFish = gameData.FishCount;
		revealedTiles = gameData.RevealedTiles;

		PlaceCoast(coastPos);
		PlacePlayer(playerPos);

		foreach (var pos in revealedTiles)
		{
			fow.EraseCell(pos);
		}
	}

	private void PopulateData()
	{
		List<Vector2I> allTiles = [];

		for (int x = -3; x <= 2; x++)
		{
			for (int y = -3; y <= 2; y++)
			{
				allTiles.Add(new Vector2I(x, y));
			}
		}

		Utils.ShuffleList(allTiles);

		PlaceCoast(allTiles[0]);
		PlacePlayer(allTiles[1]);
		coastPos = allTiles[0];
		playerPos = allTiles[1];

		var rng = new RandomNumberGenerator();
		rng.Randomize();

		//it populates the coast tile when it doesnt need to but idc man
		foreach (var tile in allTiles)
		{

			int fish = rng.RandiRange((int)fishRate.X, (int)fishRate.Y);
			int bubble = rng.RandiRange((int)bubbleRate.X, (int)bubbleRate.Y);
			int bomb = rng.RandiRange((int)bombRate.X, (int)bombRate.Y);
			dict[tile] = new Vector3I(fish, bubble, bomb);
		}
	}

	private void PlacePlayer(Vector2I gridPos)
	{
		fow.EraseCell(gridPos);
		Vector2 worldPos = map.MapToLocal(gridPos);
		player.Position = worldPos;
	}

	private void PlaceCoast(Vector2I gridPos)
	{
		map.SetCell(gridPos, 0, new Vector2I(2, 0));
	}

	public void SelectTile(Vector2I gridPos)
	{
		if (!IsReachable(playerPos, gridPos))
		{
			info.Text = "";
			button.Text = "Tile Unreachable";
			return;
		}

		if (fow.GetCellSourceId(gridPos) == -1)
		{
			info.Text = $"Fish: {dict[gridPos].X}\nBubbles: {dict[gridPos].Y}\nBombs: {dict[gridPos].Z}";
			if (!revealedTiles.Contains(gridPos)) revealedTiles.Add(gridPos);
			if (gridPos == playerPos)
			{
				button.Text = "Dive";
			}
			else
			{
				button.Text = "Travel";
			}
		}
		else
		{
			button.Text = "Travel";
			info.Text = "";
		}
	}

	private bool IsReachable(Vector2I playerPos, Vector2I gridPos)
	{
		int px = playerPos.X;
		int py = playerPos.Y;
		int gx = gridPos.X;
		int gy = gridPos.Y;

		if (((gx == px + 1 && gy == py) ||
			(gx == px - 1 && gy == py) ||
			(gy == py + 1 && gx == px) ||
			(gy == py - 1 && gx == px) ||
			(playerPos == gridPos)) &&
			map.GetCellSourceId(gridPos) != -1) return true;
		return false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse
			&& mouse.Pressed
			&& mouse.ButtonIndex == MouseButton.Left)
		{
			Vector2 worldPos = GetGlobalMousePosition();

			var gridSize = 32;
			gridPos = new Vector2I(
				Mathf.FloorToInt(worldPos.X / gridSize),
				Mathf.FloorToInt(worldPos.Y / gridSize)
			);

			SelectTile(gridPos);
		}
	}
}
