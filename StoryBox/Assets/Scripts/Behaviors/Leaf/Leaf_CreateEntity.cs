using System.Collections.Generic;
using UnityEngine;

public class Leaf_CreateEntity : Behavior {

	public GameObject entityPrefab;

	[Header("Write Keys")]
	public string childEntitiesKey = "children";

	public override NodeStatus Act(Entity entity, Memory memory) {
		if (memory[childEntitiesKey] == null)
			memory[childEntitiesKey] = new List<Entity>();

		List<Vector2Int> neighbors = World.GetAdjacentCoords(entity.position.x, entity.position.y);
		neighbors.Shuffle();

		foreach (Vector2Int neighbor in neighbors) {
			Entity child = EntityManager.instance.CreateEntity(entityPrefab, neighbor.x, neighbor.y, entity.faction);
			if (child != null) {
				(memory[childEntitiesKey] as List<Entity>).Add(child);
				return NodeStatus.Success;
			}
		}
		return NodeStatus.Failure;
	}
}