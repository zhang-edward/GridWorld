using UnityEngine;

public class Leaf_Transform : Behavior {

	public EntityData data;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string entityKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = entityKey != "" ? memory[entityKey] as Entity : entity;
		target.TransformTo(data);

		return NodeStatus.Success;
	}
}