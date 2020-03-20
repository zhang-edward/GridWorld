using System.Collections.Generic;
using UnityEngine;

public class Leaf_MovePath : Behavior {

	public Behavior movement;

	[Header("Read Keys")]
	public string pathKey = "path";

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	private Stack<Vector2Int> cachedPath;

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		movement.Init(entity, memory);
	}

	public override NodeStatus Act() {
		if (cachedPath == null)
			cachedPath = memory[pathKey] as Stack<Vector2Int>;

		if (cachedPath.Count > 0) {
			Vector2Int dest = cachedPath.Pop();
			memory[movementKey] = dest;

			switch (movement.Act()) {
				// Move was successful
				case NodeStatus.Success:
					// If the path is now empty, return success
					if (cachedPath.Count == 0) {
						cachedPath = null;
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
					cachedPath.Push(dest);
					return NodeStatus.Failure;
			}
		}
		// If the cached path has a length of 0, just return success
		Debug.LogWarning($"{this} retrieved a cached path with length 0, this is probably an error", this);
		cachedPath = null;
		return NodeStatus.Success;
	}
}