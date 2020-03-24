using System.Collections.Generic;
using UnityEngine;

public class Leaf_BecomeChild : Behavior {

	[Header("Target Entity")]
	[Tooltip("Entity")]
	public string targetEntity;

    [Header("Write Keys to target")]
	public string targetWriteKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = (Entity)memory[targetEntity];
		List<Entity> list = target.memory[targetWriteKey] as List<Entity>;
		if (list == null)
			list = new List<Entity>();
		list.Add(entity);
		target.memory[targetWriteKey] = list;

		return NodeStatus.Success;
	}
}