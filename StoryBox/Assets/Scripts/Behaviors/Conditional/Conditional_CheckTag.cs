using System.Collections.Generic;
using UnityEngine;

public class Conditional_CheckTag : Behavior {

	public List<string> tagValues;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string entityKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = (Entity)memory[entityKey];

		foreach (string tagValue in tagValues) {
			if (!target.tags.Contains(tagValue))
				return NodeStatus.Failure;
		}
		return NodeStatus.Success;
	}
}