using System.Collections.Generic;
using UnityEngine;

public class Leaf_BecomeChild : Behavior {

	[Header("Target Entity")]
	[Tooltip("Entity")]
	public string targetEntity;

    [Header("Write Keys to target")]
	public string targetWriteKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = (Entity)memory[targetEntity];
		target.AddChild(targetWriteKey, entity);
		return NodeStatus.Success;
	}
}