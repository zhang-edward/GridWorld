using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Returns a list of coordinates for which a tilemap has id
/// </summary>
public class Leaf_TilemapSensor : Behavior {

	public enum TilemapMode {
		Terrain,
		Resources
	}
	public TilemapMode tilemap;

	public List<int> ids;
	public int range = World.WORLD_SIZE;
	public bool noEntity = false;

	[Header("Write Keys")]
	[Tooltip("List<Vector2Int>")]
	public string positionsKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> selected = new List<Vector2Int>();
		bool found = false;
		int[, ] map = null;
		// Select which tilemap to look in
		switch (tilemap) {
			case TilemapMode.Resources:
				map = entity.world.ResourcesMap;
				break;
			case TilemapMode.Terrain:
				map = entity.world.BaseMap;
				break;
		}
		// Search tilemap for ids
		int xx = entity.position.x;
		int yy = entity.position.y;
		for (int y = yy - range; y < yy + range; y++) {
			for (int x = xx - range; x < xx + range; x++) {
				if (World.InBounds(x, y) && Detects(map[y, x], x, y)) {
					selected.Add(new Vector2Int(x, y));
					found = true;
				}
			}
		}
		if (found) {
			memory[positionsKey] = selected;
			return NodeStatus.Success;
		} else
			return NodeStatus.Failure;
	}

	private bool Detects(int tile, int x, int y) {
		if (noEntity && EntityManager.instance.EntityExistsAt(x, y))
			return false;
		else if (!ids.Contains(tile))
			return false;
		return true;
	}
}