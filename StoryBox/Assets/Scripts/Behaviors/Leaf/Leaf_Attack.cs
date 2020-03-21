using UnityEngine;

public class Leaf_Attack : Behavior {

	public int range = 1;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey = "attack_target";

	public override NodeStatus Act(Entity entity, Memory memory) {
		Entity attackTarget = memory[targetKey] as Entity;
		int attackPower = entity.attack;

		if (attackTarget.health > 0 &&
			entity.position.ManhattanDistance(attackTarget.position) <= range) {
			attackTarget.health -= attackPower;

			if (attackTarget.health > 0)
				return NodeStatus.Running;
			else {
				attackTarget = null;
				return NodeStatus.Success;
			}
		} else {
			attackTarget = null;
			return NodeStatus.Failure;
		}
	}
}