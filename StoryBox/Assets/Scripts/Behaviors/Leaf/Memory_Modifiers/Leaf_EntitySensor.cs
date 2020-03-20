using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySensor : Behavior {

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;
	public List<int> factions = new List<int>();

	[Header("Other properties")]
	public int range = 100;

	[Header("Write Keys")]
	public string entitiesKey = "entities";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Entity> entities = EntityManager.instance.entities;
		List<Entity> entitesInRange = new List<Entity>();

		foreach (Entity other in entities) {
			if (other == entity || !Detects(entity, other)) continue;
			if (Vector2Int.Distance(other.position, entity.position) < range)
				entitesInRange.Add(other);
		}

		memory[entitiesKey] = entitesInRange;
		return NodeStatus.Success;
	}

	private bool Detects(Entity entity, Entity other) {
		if (allies && other.faction == entity.faction)
			return true;
		else if (enemies && other.faction != entity.faction)
			return true;
		else if (factions.Contains(other.faction))
			return true;
		else
			return false;
	}
}