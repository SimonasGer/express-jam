using Godot;
using System;

public partial class PlayerJump : Sprite2D
{
	private const float gravity = 1.0f;
	private float force = 10.0f;
	private bool start = false;
	private const float maxRotation = -Mathf.Pi;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("jump"))
		{
			start = true;
		}
		if (start)
		{
			force -= gravity * (float)delta * 10;
			Position += new Vector2(-2.0f, -force) * (float)delta * 10;
			if (Rotation > maxRotation)
			{
				Rotation -= 0.025f;
			}
		}
		if (Position.Y > 400)
		{
			GetTree().ChangeSceneToFile("res://scenes/underwater.tscn");
		}
	}
}
