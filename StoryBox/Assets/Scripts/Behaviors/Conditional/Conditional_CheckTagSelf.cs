using UnityEngine;

public class Conditional_CheckTagSelf : Behavior {

	public string tagValue;

	public override NodeStatus Act(Entity entity, Memory memory) {
		if (entity.tags.Contains(tagValue))
			return NodeStatus.Success;
		else
			return NodeStatus.Failure;
	}
}