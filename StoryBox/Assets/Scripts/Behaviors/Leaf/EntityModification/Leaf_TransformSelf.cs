using UnityEngine;

public class Leaf_TransformSelf : Behavior {

	public EntityData data;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		entity.TransformTo(data);
		return NodeStatus.Success;
	}
}