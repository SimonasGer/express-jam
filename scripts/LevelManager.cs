using Godot;
using System;

public partial class LevelManager : Node2D
{
	private Player player;
	private readonly RandomNumberGenerator rng = new();
	public int maxFish, maxBombs, maxBubbles, fishCount = 0, caughtFish, bombCount = 0, bubbleCount = 0;
	private PackedScene fishScene = (PackedScene)ResourceLoader.Load("res://scenes/fish.tscn");
	private PackedScene bombScene = (PackedScene)ResourceLoader.Load("res://scenes/bomb.tscn");
	private PackedScene bubbleScene = (PackedScene)ResourceLoader.Load("res://scenes/bubble.tscn");
	private Node2D thing;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameData = GetNode<GameData>("/root/GameData");
		caughtFish = gameData.FishCount;
		rng.Randomize();
		player = GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		maxFish = (int)(player.Position.Y / 100);
		maxBombs = (int)(player.Position.Y / 300);
		maxBubbles = (int)(player.Position.Y / 200);

		SpawnEntity("fish", fishCount, maxFish);
		SpawnEntity("bomb", bombCount, maxBombs);
		SpawnEntity("bubble", bubbleCount, maxBubbles);
	}

	private void SpawnEntity(string entity, int count, int max)
	{
		if (count >= max) return;

		switch (entity)
		{
			case "fish":
				thing = (Fish)fishScene.Instantiate();
				break;
			case "bomb":
				thing = (Bomb)bombScene.Instantiate();
				break;
			case "bubble":
				thing = (Bubble)bubbleScene.Instantiate();
				break;
			default:
				thing = (Fish)fishScene.Instantiate();
				break;
		}


		var cam = GetViewport().GetCamera2D();
		Rect2 vis = GetViewport().GetVisibleRect();
		Vector2 center = cam != null ? cam.GlobalPosition : vis.GetCenter();

		float margin = 64f;
		float thickness = 128f;
		float halfDiag = 0.5f * Mathf.Sqrt(vis.Size.X * vis.Size.X + vis.Size.Y * vis.Size.Y);

		float rMin = halfDiag + margin;
		float rMax = rMin + thickness;

		float angle = rng.RandfRange(0f, Mathf.Tau);
		float radius = rng.RandfRange(rMin, rMax);

		Vector2 pos = center + Vector2.FromAngle(angle) * radius;
		thing.GlobalPosition = pos;

		AddChild(thing);
		count++;
		thing.TreeExited += () => count--;
		switch (entity)
		{
			case "fish":
				fishCount = count;
				break;
			case "bomb":
				bombCount = count;
				break;
			case "bubble":
				bubbleCount = count;
				break;
			default:
				fishCount = count;
				break;
		}
	}
}
