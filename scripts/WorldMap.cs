using Godot;
using System;
using System.Collections.Generic;

public partial class WorldMap : Node2D
{
	private TileMapLayer fow, map;
	private Vector2 fishRate = new(50, 150); //will adjust numbers later
	private Vector2 bubbleRate = new(50, 150);
	private Vector2 bombRate = new(50, 150);
	private Sprite2D player;
	private Button button;
	private readonly Dictionary<Vector2I, Vector3I> dict;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetNode<TileMapLayer>("Map");
		fow = GetNode<TileMapLayer>("Fow");
		player = GetNode<Sprite2D>("Player");
		button = GetNode<Button>("CanvasLayer/Panel/Button");
		PopulateData();
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
		fow.SetCell(gridPos, -1);
		Vector2 worldPos = map.MapToLocal(gridPos);
		player.Position = worldPos;
	}

	private void PlaceCoast(Vector2I gridPos)
	{
		map.SetCell(gridPos, 0, new Vector2I(2, 0));
	}

	private void Reveal(Vector2I gridPos)
	{
		//later
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse
			&& mouse.Pressed
			&& mouse.ButtonIndex == MouseButton.Left)
		{
			Vector2 worldPos = GetGlobalMousePosition();

			// Now map to grid
			var gridSize = 32; // your tile size
			var gridPos = new Vector2I(
				Mathf.FloorToInt(worldPos.X / gridSize),
				Mathf.FloorToInt(worldPos.Y / gridSize)
			);

			Reveal(gridPos);
		}
	}
}
