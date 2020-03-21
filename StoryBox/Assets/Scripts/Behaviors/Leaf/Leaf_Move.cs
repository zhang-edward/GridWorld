using System.Collections.Generic;
using UnityEngine;
public class Leaf_Move : Behavior {

	[Header("Read Keys")]
	public string movementKey = "move_dest";

	public override NodeStatus Act(Entity entity, Memory memory) {
		int[, ] map = entity.world.BaseMap;

		Vector2Int dest = (Vector2Int) memory[movementKey];
		if (entity.allowedTiles.Contains(map[dest.y, dest.x]) &&
			!EntityManager.instance.CapacityReachedAt(dest.x, dest.y)) {
			entity.Move(dest.x, dest.y);
			return NodeStatus.Success;
		}
		return NodeStatus.Failure;
	}
}