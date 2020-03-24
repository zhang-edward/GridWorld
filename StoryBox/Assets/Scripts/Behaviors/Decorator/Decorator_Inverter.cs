using System.Collections.Generic;
using UnityEngine;

public class Decorator_Inverter : Behavior {

	public Behavior subBehavior;

	public override void Init() {
		subBehavior.Init();
	}

	public override string PrintTreeTraversal(Stack<int> stack, Entity entity) {
		return $"{gameObject.name} (Inverter) \n{subBehavior.PrintTreeTraversal(stack, entity)}";
	}

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