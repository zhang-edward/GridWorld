using System.Collections.Generic;
using UnityEngine;

public class Conditional_CheckTag : Behavior {

	public List<string> tagValues;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string entityKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = entityKey != "" ? (Entity)memory[entityKey] : entity;

		foreach (string tagValue in tagValues) {
			if (!target.tags.Contains(tagValue))
				return NodeStatus.Failure;
		}
		return NodeStatus.Success;
	}
}