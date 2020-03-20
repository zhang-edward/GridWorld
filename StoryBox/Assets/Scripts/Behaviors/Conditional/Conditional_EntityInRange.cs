using UnityEngine;
public class Conditional_EntityInRange : Behavior {

	public int range = 1;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey = "target";

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity other = memory[targetKey] as Entity;
		if (Vector2Int.Distance(entity.position, other.position) <= range)
			return NodeStatus.Success;
		else
			return NodeStatus.Failure;
	}
}