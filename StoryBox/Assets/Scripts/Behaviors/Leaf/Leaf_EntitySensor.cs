using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySensor : Behavior {

	int range = 100;
	[Header("Write Keys")]
	public string entitiesKey = "entities";

	public override NodeStatus Act() {
		List<Entity> entities = GameManager.instance.entities;
		List<Entity> entitesInRange = new List<Entity>();
		
		foreach (Entity other in entities) {
			if (other == entity || entity.faction == other.faction) continue;
			if (Vector2Int.Distance(other.position, entity.position) < range)
				entitesInRange.Add(other);
		}
		
		memory[entitiesKey] = entitesInRange;
		return NodeStatus.Success;
	}
}