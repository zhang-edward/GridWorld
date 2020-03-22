using UnityEngine;

public class Decorator_Succeeder : Behavior {

	public Behavior subBehavior;

	public override NodeStatus Act(Entity entity, Memory memory) {
		subBehavior.Act(entity, memory);
		return NodeStatus.Success;
	}
}