using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float speed = 300.0f;
	private Sprite2D sprite;
	[Export] public ColorRect DepthOverlay;
	private const float MaxDepth = 10000f;


	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
	}
	public override void _Process(double delta)
	{
		float depth = Mathf.Clamp((float)Position.Y / MaxDepth, 0f, 1f);
		DepthOverlay.Color = new Color(0, 0, 0, depth);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 dir = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			dir.X += 1;
			sprite.Rotation = Mathf.Pi / 2;
		}
		if (Input.IsActionPressed("move_left"))
		{
			dir.X -= 1;
			sprite.Rotation = -Mathf.Pi / 2;
		}
		if (Input.IsActionPressed("move_down"))
		{
			dir.Y += 1;
			sprite.Rotation = -Mathf.Pi;
		}
		if (Input.IsActionPressed("move_up"))
		{
			dir.Y -= 1;
			sprite.Rotation = 0;
		}

		Velocity = dir.Normalized() * speed;
		MoveAndSlide();
	}
}
