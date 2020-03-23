using UnityEngine;
using System.Collections;

public class Leaf_Animation : Behavior {

	public string animationKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		entity.PlayAnimation(animationKey);
		print("Executed this shit!");
		return NodeStatus.Success;
	}
}
