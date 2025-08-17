using Godot;
using System;

public partial class Bomb : CharacterBody2D
{
	private Area2D area2D;
	private Player player;
	private Label label;
	private LevelManager levelManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		player = GetNode<Player>("/root/Underwater/Player");
		label = GetNode<Label>("/root/Underwater/CanvasLayer/Panel/HealthLabel");
		levelManager = GetNode<LevelManager>("/root/Underwater");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Despawn();
		if (area2D.GetOverlappingBodies().Contains(player))
		{
			player.health--;
			label.Text = $"Health: {player.health}/{player.maxHealth}";
			player.Die();
			QueueFree();
		}
	}

	private void Despawn()
	{
		if (Position.DistanceTo(player.Position) > 1500)
		{
			QueueFree();
		}
	}

}
