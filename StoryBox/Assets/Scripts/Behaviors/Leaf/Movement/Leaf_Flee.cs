using UnityEngine;
using System.Collections.Generic;

public class Leaf_Flee : Behavior {

	public Behavior movement;
	public int safeDistance;

	[Header("Read Keys")]
	public string entityKey;

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = (Entity)memory[entityKey];
		Vector2 awayDirection = entity.position - target.position;
		Vector2 dest;
		if (awayDirection == Vector2Int.zero)
			dest = World.GetAdjacentCoords(entity.position.x, entity.position.y)[Random.Range(0, 4)];
		else
			dest = entity.position + awayDirection.normalized;
		memory[movementKey] = new Vector2Int(Mathf.RoundToInt(dest.x), Mathf.RoundToInt(dest.y));

		switch (movement.ExecuteAction(entity, memory)) {
			// Move was successful
			case NodeStatus.Success:
				return entity.position.ManhattanDistance(target.position) >= safeDistance ?
					NodeStatus.Success : NodeStatus.Running;
			// Movement behavior is somehow multi-step
			case NodeStatus.Running:
				return NodeStatus.Running;
			// Movement behavior failed
			case NodeStatus.Failure:
				return NodeStatus.Failure;
			default:
				return NodeStatus.Failure;
		}
	}
}
