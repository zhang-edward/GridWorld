using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySensor : Behavior {

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;
	public List<string> tags;
	[Tooltip("Check if this entity has the tag placed by Leaf_TagUniquely")]
	public bool checkUniqueTag;

	[Header("Other properties")]
	public int range;

	[Header("Write Keys")]
	public string entitiesKey = "entities";
	public string countKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		List<Entity> answer = new List<Entity>();

		int xx = entity.position.x;
		int yy = entity.position.y;
		for (int y = yy - range; y <= yy + range; y++) {
			for (int x = xx - range; x <= xx + range; x++) {
				if (World.InBounds(x, y)) {
					List<Entity> entities = EntityManager.instance.GetEntitiesAt(x, y);
					foreach (Entity other in entities) {
						if (Detects(entity, other)) {
							answer.Add(other);
						}
					}
				}
			}
		}
		memory[entitiesKey] = answer;
		memory[countKey] = answer.Count;
		return NodeStatus.Success;
	}

	private bool Detects(Entity entity, Entity other) {
		return CheckTags(entity, other) &&
			((allies && other.faction == entity.faction) ||
			(enemies && other.faction != entity.faction));
	}

	private bool CheckTags(Entity entity, Entity other) {
		if (checkUniqueTag && !other.tags.Contains(entity.uniqueTag))
			return false;
		foreach (string tag in tags) {
			if (!other.tags.Contains(tag))
				return false;
		}
		return true;
	}
}