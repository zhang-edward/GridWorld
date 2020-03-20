using UnityEngine;

public class Leaf_Attack : Behavior {

	public int baseAttack = 1;
	public int range = 1;

	[Header("Read and Write Keys")]
	[Tooltip("int")]
	public string attackKey = "attack";

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey = "attack_target";

	private Entity cachedAttackTarget;

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		memory[attackKey] = baseAttack;
	}

	public override NodeStatus Act() {
		if (cachedAttackTarget == null) {
			cachedAttackTarget = memory[targetKey] as Entity;
		}
		int attackPower = (int) memory[attackKey];

		if (cachedAttackTarget.health > 0 &&
			entity.position.ManhattanDistance(cachedAttackTarget.position) <= range) {
			cachedAttackTarget.health -= attackPower;

			if (cachedAttackTarget.health > 0)
				return NodeStatus.Running;
			else {
				cachedAttackTarget = null;
				return NodeStatus.Success;
			}
		} else {
			cachedAttackTarget = null;
			return NodeStatus.Failure;
		}
	}
}