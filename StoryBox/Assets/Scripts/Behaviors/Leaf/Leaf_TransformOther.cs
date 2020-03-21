using UnityEngine;

public class Leaf_TransformOther : Behavior {

	public GameObject entityPrefab;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string entityKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = memory[entityKey] as Entity;
		target.Kill();
		Entity child = EntityManager.instance.CreateEntity(entityPrefab, entity.position.x, entity.position.y, entity.faction);
		if (child != null) {
			return NodeStatus.Success;
		}
		return NodeStatus.Failure;
	}
}