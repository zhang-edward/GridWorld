using System.Collections.Generic;
using UnityEngine;

public class Leaf_CreateEntity : Behavior {

	public EntityData data;

	[Header("Read Keys")]
	[Tooltip("Vector2Int")]
	public string positionKey = "position";

	[Header("Write Keys")]
	public string childEntitiesKey = "children";
	public string childKey = "child";

	public override NodeStatus Act(Entity entity, Memory memory) {
		Vector2Int position = (Vector2Int)memory[positionKey];
		Entity child = EntityManager.instance.CreateEntity(data, position.x, position.y, entity.faction);

		if (child != null) {
			if (childEntitiesKey != "") {
				entity.AddChild(childEntitiesKey, child);
			}
			memory[childKey] = child;
			return NodeStatus.Success;
		}
		return NodeStatus.Failure;
	}
}