using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float speed = 300.0f;
	private Sprite2D sprite;
	[Export] public ColorRect DepthOverlay;
	[Export] public HSlider breathSlider;
	private const float MaxDepth = 10000f;
	public int health = 3, maxHealth = 3;
	public float breath = 60.0f, maxBreath = 60.0f;

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
		Movement();
		Breath(delta);
	}

	private void Movement()
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

	private void Breath(double delta)
	{
		breath -= (float)delta;
		breathSlider.Value = breath;
		if (breath < 0)
		{
			GetTree().ChangeSceneToFile("res://scenes/surface.tscn");
		}
	}

	public void Die()
	{
		if (health == 0)
		{
			GetTree().ChangeSceneToFile("res://scenes/surface.tscn");
		}
	}
}
