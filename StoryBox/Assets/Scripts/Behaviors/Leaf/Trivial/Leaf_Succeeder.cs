using UnityEngine;

public class Leaf_Succeeder : Behavior {

	public override NodeStatus Act(Entity entity, Memory memory) {
		return NodeStatus.Success;
	}
}