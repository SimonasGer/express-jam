using Godot;
using System;

public partial class LevelManager : Node2D
{
	private Player player;
	private readonly RandomNumberGenerator rng = new();
	public int maxFish, maxBombs, fishCount = 0, bombCount = 0;
	private PackedScene fishScene = (PackedScene)ResourceLoader.Load("res://scenes/fish.tscn");
	private Fish fish;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng.Randomize();
		player = GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		maxFish = (int)(player.Position.Y / 50);
		
		SpawnFish();
	}

	private void SpawnFish()
	{
		if (fishCount >= maxFish) return;

		var fish = (Fish)fishScene.Instantiate();

		// Camera center (fallback to visible rect center if no camera)
		var cam = GetViewport().GetCamera2D();
		Rect2 vis = GetViewport().GetVisibleRect();
		Vector2 center = cam != null ? cam.GlobalPosition : vis.GetCenter();

		// Build a ring just outside the screen's half-diagonal
		float margin = 64f;         // how far beyond the corners
		float thickness = 128f;     // ring thickness
		float halfDiag = 0.5f * Mathf.Sqrt(vis.Size.X * vis.Size.X + vis.Size.Y * vis.Size.Y);

		float rMin = halfDiag + margin;
		float rMax = rMin + thickness;

		float angle = rng.RandfRange(0f, Mathf.Tau);
		float radius = rng.RandfRange(rMin, rMax);

		Vector2 pos = center + Vector2.FromAngle(angle) * radius;
		fish.GlobalPosition = pos;  // global because we used camera/world space

		AddChild(fish);
		fishCount++;

		// Optional: aim fish toward the center so it "swims in"
		if (fish is Node2D n2d)
		{
			Vector2 dir = (center - pos).Normalized();
			// e.g., if your Fish has a SetDirection or Velocity:
			// fish.Velocity = dir * fishSpeed;
			// or rotate sprite:
			// n2d.Rotation = dir.Angle();
		}

		// Keep count honest when fish despawn
		fish.TreeExited += () => fishCount--;
	}
}
