using UnityEngine;
using System.Collections.Generic;

public class Leaf_Wander : Behavior {

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	public override NodeStatus Act() {
		List<Vector2Int> neighbors = World.GetAdjacentCoords(entity.position.x, entity.position.y);
		memory[movementKey] = neighbors[Random.Range(0, neighbors.Count)];
		return NodeStatus.Success;
	}
}