using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySensor : Behavior {

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;
	public List<int> factions;
	public List<string> tags;

	[Header("Other properties")]
	public int range = 100;

	[Header("Write Keys")]
	public string entitiesKey = "entities";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Entity> answer = new List<Entity>();

		bool found = false;
		int xx = entity.position.x;
		int yy = entity.position.y;
		for (int y = yy - range; y < yy + range; y++) {
			for (int x = xx - range; x < xx + range; x++) {
				if (World.InBounds(x, y)) {
					List<Entity> entities = EntityManager.instance.GetEntitiesAt(x, y);
					foreach (Entity other in entities) {
						if (Detects(entity, other)) {
							answer.Add(other);
							found = true;
						}
					}
				}
			}
		}
		if (found) {
			memory[entitiesKey] = answer;
			return NodeStatus.Success;
		} 
		else
			return NodeStatus.Failure;
	}

	private bool Detects(Entity entity, Entity other) {
		if (!TagsMatch(other))
			return false;
		if (allies && other.faction == entity.faction)
			return true;
		else if (enemies && other.faction != entity.faction)
			return true;
		else if (factions.Contains(other.faction))
			return true;
		else
			return false;
	}

	private bool TagsMatch(Entity other) {
		foreach (string tag in tags) {
			if (!other.tags.Contains(tag))	
				return false;
		}
		return true;
	}
}