using Godot;
using System;

public partial class Bubble : CharacterBody2D
{
	private Area2D area2D;
	private Player player;
	private LevelManager levelManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		player = GetNode<Player>("/root/Underwater/Player");
		levelManager = GetNode<LevelManager>("/root/Underwater");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Despawn();
		if (area2D.GetOverlappingBodies().Contains(player))
		{
			levelManager.bubbleCount--;
			player.breath += 10.0f;
			if (player.breath > player.maxBreath)
			{
				player.breath = player.maxBreath;
			}
			QueueFree();
		}
	}

	private void Despawn()
	{
		if (Position.DistanceTo(player.Position) > 1500)
		{
			levelManager.bubbleCount--;
			QueueFree();
		}
	}

}
