using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySelector : Behavior {

	[Header("Read Keys")]
	[Tooltip("List<Entity>")]
	public string entitiesKey = "entities";

	[Header("Write Keys")]
	[Tooltip("Entity")]
	public string writeKey = "destination";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		// Precondition: entities list will always have > 0 elements
		List<Entity> entities = memory[entitiesKey] as List<Entity>;
		Entity selected = SelectNearest(entities, entity);
		if (selected != null) {
			memory[writeKey] = selected;
			return NodeStatus.Success;
		}
		else {
			return NodeStatus.Failure;
		}
	}

	private Entity SelectNearest(List<Entity> entities, Entity entity) {
		entities.Shuffle(); // Shuffle so we choose randomly between equidistant entities
		int minDist = int.MaxValue;
		Entity selected = null;
		foreach (Entity other in entities) {
			int dist = entity.position.ManhattanDistance(other.position);
			if (dist < minDist) {
				minDist = dist;
				selected = other;
			}
		}
		return selected;
	}
}