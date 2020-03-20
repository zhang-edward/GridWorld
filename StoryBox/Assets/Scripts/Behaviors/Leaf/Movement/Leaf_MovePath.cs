using System.Collections.Generic;
using UnityEngine;

public class Leaf_MovePath : Behavior {

	public Behavior movement;

	[Header("Read Keys")]
	public string pathKey = "path";

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	public override NodeStatus Act(Entity entity, Memory memory) {
		Stack<Vector2Int> path = memory[pathKey] as Stack<Vector2Int>;

		if (path.Count > 0) {
			Vector2Int dest = path.Pop();
			memory[movementKey] = dest;

			switch (movement.Act(entity, memory)) {
				// Move was successful
				case NodeStatus.Success:
					// If the path is now empty, return success
					if (path.Count == 0) {
						path = null;
						return NodeStatus.Success;
					}
					// Else, we keep running
					else {
						return NodeStatus.Running;
					}
					// Movement behavior is somehow multi-step
				case NodeStatus.Running:
					return NodeStatus.Running;
					// Movement behavior failed
				case NodeStatus.Failure:
					path.Push(dest);
					return NodeStatus.Failure;
			}
		}
		// If the cached path has a length of 0, just return success
		Debug.LogWarning($"{this} retrieved a cached path with length 0, this is probably an error", this);
		path = null;
		return NodeStatus.Success;
	}
}