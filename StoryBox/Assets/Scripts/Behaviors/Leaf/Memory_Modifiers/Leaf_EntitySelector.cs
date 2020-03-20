using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySelector : Behavior {

	[Header("Read Keys")]
	[Tooltip("List<Entity>")]
	public string entitiesKey = "entities";

	[Header("Write Keys")]
	[Tooltip("Entity")]
	public string writeKey = "destination";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Entity> entities = memory[entitiesKey] as List<Entity>;

		foreach (Entity other in entities) {
			if (other.faction != entity.faction) {
				memory[writeKey] = other;
				return NodeStatus.Success;
			}
		}
		return NodeStatus.Failure;
	}
}