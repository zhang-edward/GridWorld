using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySelector : Behavior {

	[Header("Read Keys")]
	public string entitiesKey = "entities";

	[Header("Write Keys")]
	public string writeKey = "destination";

	public override NodeStatus Act() {
		List<Entity> entities = memory[entitiesKey] as List<Entity>;

		foreach (Entity other in entities) {
			if (other.faction != entity.faction) {
				memory[writeKey] = other.position;
				return NodeStatus.Success;
			}
		}
		return NodeStatus.Failure;
	}
}