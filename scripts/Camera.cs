using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] private Node2D player;
	[Export] private Vector2 worldMin = new(-1000, 0);
	[Export] private int worldMax = 1000;
	public override void _Ready()
	{
		MakeCurrent();
	}

	public override void _Process(double delta)
	{
		var pxSize = GetViewport().GetVisibleRect().Size;

	// Convert to world units (account for camera zoom!)
	// If cam.Zoom = (2,2) you see half as much; divide by zoom.
		var half = new Vector2(pxSize.X / (2f * Zoom.X), pxSize.Y / (2f * Zoom.Y));

		Vector2 target = player.GlobalPosition;
		target.X = Mathf.Clamp(target.X, worldMin.X + half.X, worldMax - half.X);
		target.Y = Mathf.Clamp(target.Y, worldMin.Y + half.Y, 99999);

		GlobalPosition = target;
	}
}
