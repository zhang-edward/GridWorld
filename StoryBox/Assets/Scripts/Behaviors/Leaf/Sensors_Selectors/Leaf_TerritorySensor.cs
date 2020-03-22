using System.Collections.Generic;
using UnityEngine;

public class Leaf_TerritorySensor : Behavior {

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;

	[Header("Other Properties")]
	public int range = 100;

	[Header("Write Keys")]
	public string positionsKey = "positions";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> selected = new List<Vector2Int>();
		bool found = false;
		TerritoryMap.TerritoryData[, ] map = TerritoryMap.instance.map;

		// Search tilemap for ids
		int xx = entity.position.x;
		int yy = entity.position.y;
		for (int y = yy - range; y < yy + range; y++) {
			for (int x = xx - range; x < xx + range; x++) {
				if (World.InBounds(x, y) && Detects(entity.faction, map[y,x].control)) {
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

	private bool Detects(int myFaction, int otherFaction) {
		if (allies && otherFaction == myFaction)
			return true;
		else if (enemies && otherFaction != myFaction)
			return true;
		else
			return false;
	}
}