using System.Collections.Generic;
using UnityEngine;

public class Leaf_CreateEntity : Behavior {

	public GameObject entityPrefab;

	private List<Entity> children = new List<Entity>();

	[Header("Write Keys")]
	public string childEntitiesKey = "children";

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		memory[childEntitiesKey] = children;
	}

	public override NodeStatus Act() {
		List<Vector2Int> neighbors = World.GetAdjacentCoords(entity.position.x, entity.position.y);
		neighbors.Shuffle();

		foreach (Vector2Int neighbor in neighbors) {
			Entity child = EntityManager.instance.CreateEntity(entityPrefab, neighbor.x, neighbor.y, Random.Range(0, 5));
			if (child != null) {
				children.Add(child);
				return NodeStatus.Success;
			}
		}
		return NodeStatus.Failure;
	}
}