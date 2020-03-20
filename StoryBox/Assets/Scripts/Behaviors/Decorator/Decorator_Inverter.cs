using UnityEngine;

public class Decorator_Inverter : Behavior {

	public Behavior subBehavior;

	public override NodeStatus Act(Entity entity, Memory memory) {
		NodeStatus status = subBehavior.Act(entity, memory);
		if (status == NodeStatus.Success)
			return NodeStatus.Failure;
		else if (status == NodeStatus.Failure)
			return NodeStatus.Success;
		else
			return status;
	}
}