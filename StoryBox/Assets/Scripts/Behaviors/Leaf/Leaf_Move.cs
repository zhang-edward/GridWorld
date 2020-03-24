using System.Collections.Generic;
using UnityEngine;
public class Leaf_Move : Behavior {

	public int moveRange = 1;

	[Header("Read Keys")]
	public string movementKey = "move_dest";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Vector2Int dest = (Vector2Int) memory[movementKey];
		int[, ] map = entity.world.BaseMap;

		if (dest.ManhattanDistance(entity.position) <= moveRange &&
			entity.allowedTiles.Contains(map[dest.y, dest.x]) &&
			!EntityManager.instance.CapacityReachedAt(dest.x, dest.y)) {
			entity.Move(dest.x, dest.y);
			return NodeStatus.Success;
		}
		return NodeStatus.Failure;
	}
}