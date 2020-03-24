using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntityFilter : Behavior {

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;
	public List<string> tags;
	[Tooltip("Check if this entity has the tag placed by Leaf_TagUniquely")]
	public bool checkUniqueTag;

	[Header("Read Keys")]
	public string entitiesKeyIn;

	[Header("Write Keys")]
	public string entitiesKeyOut;
	public string countKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(entitiesKeyIn, new List<Entity>());
		List<Entity> entities = (List<Entity>) memory[entitiesKeyIn];
		List<Entity> filteredEntities = new List<Entity>();

		foreach (Entity other in entities) {
			if (Detects(entity, other)) {
				filteredEntities.Add(other);
			}
		}
		memory[entitiesKeyOut] = filteredEntities;
		memory[countKey] = filteredEntities.Count;
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