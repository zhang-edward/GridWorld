using UnityEngine;

public class Leaf_TransformOther : Behavior {

	public EntityData data;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string entityKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = memory[entityKey] as Entity;
		target.InitFromData(data);
		return NodeStatus.Success;
	}
}