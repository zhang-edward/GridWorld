using UnityEngine;

public class Leaf_Move : Behavior {

	[Header("Read Keys")]
	public string movementKey = "move_dest";

	private int[, ] map;

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		map = entity.world.BaseMap;
	}

	public override NodeStatus Act() {
		Vector2Int dest = (Vector2Int)memory[movementKey];
		// if (map[dest.y, dest.x] != 0) {
			entity.Move(dest.x, dest.y);
			return NodeStatus.Success;
		// }
		// return NodeStatus.Failure;
	}
}