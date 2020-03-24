using UnityEngine;
using System.Collections;

public class Leaf_ResetAnimation : Behavior {

	protected override NodeStatus Act(Entity entity, Memory memory) {
		entity.ResetAnimation();
		return NodeStatus.Success;
	}
}
