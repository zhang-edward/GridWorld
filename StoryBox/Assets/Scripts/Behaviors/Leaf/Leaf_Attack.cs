using UnityEngine;

public class Leaf_Attack : Behavior {

	public int baseAttack = 1;
	public int range = 1;

	[Header("Read Keys")]
	public string attackKey = "attack";
	public string targetKey = "attack_target";

	private Entity cachedAttackTarget;

	public override NodeStatus Act() {
		if (cachedAttackTarget == null) {
			cachedAttackTarget = memory[targetKey] as Entity;
		}
		int attackPower = (int) memory[attackKey];

		if (cachedAttackTarget.health > 0 && 
			Vector2Int.Distance(entity.position, cachedAttackTarget.position) <= range) {
			cachedAttackTarget.health -= attackPower;

			if (cachedAttackTarget.health > 0)
				return NodeStatus.Running;
			else
				return NodeStatus.Success;
		} else {
			return NodeStatus.Failure;
		}
	}
}