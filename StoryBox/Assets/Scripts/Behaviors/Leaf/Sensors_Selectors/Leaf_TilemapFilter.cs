using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes a list of coordinates and outputs a list of coordinates for which 
/// the selected tilemap has some ids
/// </summary>
public class Leaf_TilemapFilter : Behavior {

	public enum TilemapMode {
		Terrain,
		Resources
	}
	public TilemapMode tilemap;

	public List<int> ids;
	public bool noEntity = false;

	[Header("Read Keys")]
	[Tooltip("List<Vector2Int>")]
	public string positionsKeyIn = "positions";

	[Header("Write Keys")]
	[Tooltip("List<Vector2Int>")]
	public string positionsKeyOut = "positions";

	public override NodeStatus Act(Entity entity, Memory memory) {
		// Read positions list from memory
		List<Vector2Int> positions = memory[positionsKeyIn] as List<Vector2Int>;
		bool found = false;
		// Select which tilemap to look in
		int[, ] map = null;
		switch (tilemap) {
			case TilemapMode.Resources:
				map = entity.world.ResourcesMap;
				break;
			case TilemapMode.Terrain:
				map = entity.world.BaseMap;
				break;
		}

		List<Vector2Int> filteredPositions = new List<Vector2Int>();
		// Search tilemap for ids
		foreach (Vector2Int vec in positions) {
			int x = vec.x;
			int y = vec.y;
			if (Detects(map[y, x], x, y)) {
				filteredPositions.Add(new Vector2Int(x, y));
				found = true;
			}
		}

		// Write filtered list to memory
		if (found) {
			memory[positionsKeyOut] = filteredPositions;
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