using UnityEngine;

/// <summary>
/// Repeats the subBehavior until the conditional behavior returns Success
/// </summary>
public class Decorator_RepeatUntil : Behavior {

	public Behavior conditional;
	public Behavior subBehavior;

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		conditional.Init(entity, memory);
		subBehavior.Init(entity, memory);
	}

	public override NodeStatus Act() {	
		if (conditional.Act() != NodeStatus.Success) {
			NodeStatus status = subBehavior.Act();
			if (status == NodeStatus.Failure)
				return NodeStatus.Failure;
			else
				return NodeStatus.Running;
		}
		else
			return NodeStatus.Success;
	}
}