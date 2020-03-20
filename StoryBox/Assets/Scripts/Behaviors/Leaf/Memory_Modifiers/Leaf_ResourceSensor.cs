using System.Collections.Generic;
using UnityEngine;

public class Leaf_ResourceSensor : Behavior {

	public List<int> structureIds;
	public int range = 1;

	[Header("Write Keys")]
	public string structuresKey = "structures";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> structs = new List<Vector2Int>();

		bool found = false;
		int[, ] map = entity.world.StructureMap;
		int xx = entity.position.x;
		int yy = entity.position.y;
		for (int y = yy - range; y < yy + range; y++) {
			for (int x = xx - range; x < xx + range; x++) {
				if (World.InBounds(x, y) && structureIds.Contains(map[y, x])) {
					structs.Add(new Vector2Int(x, y));
					found = true;
				}
			}
		}
		if (found) {
			memory[structuresKey] = structs;
			return NodeStatus.Success;
		} else
			return NodeStatus.Failure;
	}
}