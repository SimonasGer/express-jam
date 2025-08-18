using Godot;
using System;

public partial class Fish : CharacterBody2D
{
	private Sprite2D sprite;
	private bool right;
	private Vector2 direction = Vector2.Right;
	private Area2D area2D;
	private Player player;
	private Label label;
	private const float speed = 75.0f;
	private LevelManager levelManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		area2D = GetNode<Area2D>("Area2D");
		player = GetNode<Player>("/root/Underwater/Player");
		label = GetNode<Label>("/root/Underwater/CanvasLayer/Panel/FishLabel");
		levelManager = GetNode<LevelManager>("/root/Underwater");

		right = Position.X < 0;
		if (!right)
		{
			sprite.FlipH = true;
			direction = Vector2.Left;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Despawn();
		if (area2D.GetOverlappingBodies().Contains(player))
		{
			levelManager.caughtFish++;
			label.Text = $"Fish: {levelManager.caughtFish}";
			levelManager.fishCount--;
			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = direction.Normalized() * speed;
		MoveAndSlide();
	}

	private void Despawn()
	{
		if (Position.DistanceTo(player.Position) > 1500)
		{
			levelManager.fishCount--;
			QueueFree();
		}
	}

}
