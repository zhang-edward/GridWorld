using UnityEngine;
public class Conditional_EntityIsAlive : Behavior {

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey = "target";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity other = memory[targetKey] as Entity;
		if (other != null && other.health > 0)
			return NodeStatus.Success;
		else
			return NodeStatus.Failure;
	}
}