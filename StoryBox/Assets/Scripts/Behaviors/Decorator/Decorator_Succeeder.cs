using System.Collections.Generic;
using UnityEngine;

public class Decorator_Succeeder : Behavior {

	public Behavior subBehavior;

	public override void Init() {
		subBehavior.Init();
	}

	public override string PrintTreeTraversal(Stack<int> stack, Entity entity) {
		return $"{gameObject.name} (Succeeder) \n{subBehavior.PrintTreeTraversal(stack, entity)}";
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		subBehavior.ExecuteAction(entity, memory);
		return NodeStatus.Success;
	}
}