using UnityEngine;
using System.Collections;

public class Leaf_ResetAnimation : Behavior {

	public override NodeStatus Act(Entity entity, Memory memory) {
		entity.ResetAnimation();
		return NodeStatus.Success;
	}
}
