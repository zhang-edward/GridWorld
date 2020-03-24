using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Repeats the subBehavior until the conditional behavior returns Success
/// </summary>
public class Decorator_DoUntil : Behavior {

	public Behavior conditional;
	public Behavior subBehavior;

	public override void Init() {
		conditional.Init();
		subBehavior.Init();
	}

	public override string PrintTreeTraversal(Stack<int> stack, Entity entity) {
		return $"{gameObject.name} (DoUntil) \n{subBehavior.PrintTreeTraversal(stack, entity)}";
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		if (conditional.ExecuteAction(entity, memory) != NodeStatus.Success) {
			NodeStatus status = subBehavior.ExecuteAction(entity, memory);
			if (status == NodeStatus.Failure)
				return NodeStatus.Failure;
			else
				return NodeStatus.Running;
		} else
			return NodeStatus.Success;
	}
}