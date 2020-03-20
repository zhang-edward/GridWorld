using UnityEngine;

public class Leaf_ModifyResourceMap : Behavior {

	public int from;
	public int to;

	public override NodeStatus Act(Entity entity, Memory memory) {
		if (entity.world.StructureMap[entity.position.y, entity.position.x] != from)
			return NodeStatus.Failure;
			
		entity.world.ModifyStructureMap(entity.position.x, entity.position.y, to);
		return NodeStatus.Success;
	}
}