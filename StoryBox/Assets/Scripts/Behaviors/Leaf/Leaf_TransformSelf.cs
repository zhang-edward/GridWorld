using UnityEngine;

public class Leaf_TransformSelf : Behavior {

	public EntityData data;

	public override NodeStatus Act(Entity entity, Memory memory) {
		entity.InitFromData(data);
		return NodeStatus.Success;
	}
}