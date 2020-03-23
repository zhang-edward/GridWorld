using UnityEngine;

public class Leaf_Attack : Behavior {

	public int range = 1;
	public int attackInterval = 2;
	public string animationKey;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey = "attack_target";

	private string attackTimerKey {
		get { return "attack_timer:" + GetInstanceID(); }
	}

	public override NodeStatus Act(Entity entity, Memory memory) {
		// Timer cooldown
		memory.SetDefault(attackTimerKey, attackInterval);
		int timer = (int)memory[attackTimerKey];
		timer--;
		if (timer > 0) {
			memory[attackTimerKey] = timer;
			return NodeStatus.Running;
		}
		else {
			memory[attackTimerKey] = attackInterval;
			// Continue
		}

		// Attack target
		Entity attackTarget = memory[targetKey] as Entity;
		int attackPower = entity.attack;

		if (attackTarget.health > 0 &&
			entity.position.ManhattanDistance(attackTarget.position) <= range) {
			// View
			entity.FaceTowards(attackTarget);
			entity.PlayAnimation(animationKey);
			// Decrement health
			attackTarget.Damage(attackPower);

			if (attackTarget.health > 0)
				return NodeStatus.Running;
			else {
				return NodeStatus.Success;
			}
		} else {
			return NodeStatus.Failure;
		}
	}

	public override string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack, Entity entity) {
		string ans = base.PrintTreeTraversal(stack, entity);
		Entity attackTarget = entity.memory[targetKey] as Entity;
		ans += $": {attackTarget}";
		return ans;
	}
}