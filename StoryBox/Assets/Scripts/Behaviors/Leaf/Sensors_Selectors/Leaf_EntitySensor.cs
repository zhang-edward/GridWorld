using System.Collections.Generic;
using UnityEngine;

public class Leaf_EntitySensor : Behavior {

	private static Vector2Int NONE = new Vector2Int(-1, -1);

	[Header("What to detect")]
	public bool allies = false;
	public bool enemies = true;
	public List<string> tags;
	[Tooltip("Check if this entity has the tag placed by Leaf_TagUniquely")]
	public bool checkUniqueTag;

	[Header("Other properties")]
	public int range;

	[Header("Read Keys")]
	public string searchFromKey;

	[Header("Write Keys")]
	public string entitiesKey = "entities";
	public string countKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Vector2Int startPosition = GetStartPosition(entity, memory);

		List<Entity> answer = new List<Entity>();

		int xx = startPosition.x;
		int yy = startPosition.y;
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

	private Vector2Int GetStartPosition(Entity entity, Memory memory) {
		if (searchFromKey == "")
			return entity.position;
		object obj = memory[searchFromKey];
		Vector2Int destination = NONE;
		if (obj is Entity) {
			destination = ((Entity)obj).position;
		}
		else if (obj is Vector2Int) {
			destination = (Vector2Int)obj;
		}
		return destination;
	}
}