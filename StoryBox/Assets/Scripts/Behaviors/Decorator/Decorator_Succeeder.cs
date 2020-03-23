using UnityEngine;

public class Decorator_Succeeder : Behavior {

	public Behavior subBehavior;

	public override void Init() {
		subBehavior.Init();
	}

	public override NodeStatus Act(Entity entity, Memory memory) {
		subBehavior.Act(entity, memory);
		return NodeStatus.Success;
	}
}