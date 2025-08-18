using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class GameData : Node
{
	public Dictionary<Vector2I, Vector3I> TileData { get; set; } = [];
	public Vector2I PlayerGridPos { get; set; }
	public Vector2I CoastGridPos { get; set; }
	public int FishCount { get; set; }
	public List<Vector2I> RevealedTiles { get; set; } = [];

	private const string SavePath = "user://save.json";

	public void Save()
	{
		// Flatten dictionary + revealed tiles
		var entries = new List<TileEntry>();
		foreach (var kvp in TileData)
			entries.Add(new TileEntry { Pos = new Vec2Dto(kvp.Key), Data = new Vec3Dto(kvp.Value) });

		var dto = new
		{
			PlayerGridPos = new Vec2Dto(PlayerGridPos),
			CoastGridPos = new Vec2Dto(CoastGridPos),
			FishCount,
			RevealedTiles = RevealedTiles.ConvertAll(v => new Vec2Dto(v)),
			TileEntries = entries
		};

		string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
		file.StoreString(json);
	}

	public void Load()
	{
		if (!FileAccess.FileExists(SavePath))
			return;

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
		string text = file.GetAsText();

		var dto = JsonSerializer.Deserialize<SaveDto>(text);

		PlayerGridPos = dto.PlayerGridPos.ToVector2I();
		CoastGridPos = dto.CoastGridPos.ToVector2I();
		FishCount = dto.FishCount;
		RevealedTiles = dto.RevealedTiles.ConvertAll(v => v.ToVector2I());

		TileData.Clear();
		foreach (var entry in dto.TileEntries)
			TileData[entry.Pos.ToVector2I()] = entry.Data.ToVector3I();
	}

	public class SaveDto
	{
		public Vec2Dto PlayerGridPos { get; set; }
		public Vec2Dto CoastGridPos { get; set; }
		public int FishCount { get; set; }
		public List<Vec2Dto> RevealedTiles { get; set; }
		public List<TileEntry> TileEntries { get; set; }
	}

	public class Vec2Dto
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vec2Dto() { }
		public Vec2Dto(Vector2I v) { X = v.X; Y = v.Y; }
		public Vector2I ToVector2I() => new(X, Y);
	}

	public class Vec3Dto
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public Vec3Dto() { }
		public Vec3Dto(Vector3I v) { X = v.X; Y = v.Y; Z = v.Z; }
		public Vector3I ToVector3I() => new(X, Y, Z);
	}
	
	public class TileEntry
	{
		public Vec2Dto Pos { get; set; }
		public Vec3Dto Data { get; set; }
	}
}

