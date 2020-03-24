using UnityEngine;

public class Conditional_And : Behavior {

	public Behavior[] conditions;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		for (int i = 0; i < conditions.Length; i++) {
			NodeStatus status = conditions[i].ExecuteAction(entity, memory);
			if (status == NodeStatus.Failure)
				return NodeStatus.Failure;
			else if (status == NodeStatus.Running)
				Debug.LogError("Conditional behavior should never return Running status");
		}
		return NodeStatus.Success;
	}
}