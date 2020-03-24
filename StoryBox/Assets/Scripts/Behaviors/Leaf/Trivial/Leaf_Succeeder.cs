using UnityEngine;

public class Leaf_Succeeder : Behavior {

	protected override NodeStatus Act(Entity entity, Memory memory) {
		return NodeStatus.Success;
	}
}