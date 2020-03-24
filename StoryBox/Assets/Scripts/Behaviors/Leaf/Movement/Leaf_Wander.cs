using System.Collections.Generic;
using UnityEngine;

public class Leaf_Wander : Behavior {

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> neighbors = World.GetAdjacentCoords(entity.position.x, entity.position.y);
		memory[movementKey] = neighbors[Random.Range(0, neighbors.Count)];
		return NodeStatus.Success;
	}
}