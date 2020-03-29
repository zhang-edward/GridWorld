using UnityEngine;

public class Leaf_ModifyHealth : Behavior {

	public int range = 1;
	public int interval = 2;
	public string animationKey;

	public enum Mode {
		Damage,
		Heal
	}

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string targetKey;
	public Mode mode;
	[Tooltip("int")]
	public string amountKey;
	public int defaultAmount;

	private string timerKey {
		get { return "health_modify_timer:" + GetInstanceID(); }
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		// Timer cooldown
		memory.SetDefault(timerKey, interval);
		int timer = (int)memory[timerKey];
		timer--;
		if (timer > 0) {
			memory[timerKey] = timer;
			return NodeStatus.Running;
		}
		else {
			memory[timerKey] = interval;
			// Continue
		}

		// Attack target
		if (amountKey != "")
			memory.SetDefault(amountKey, defaultAmount);
		Entity target = targetKey != "" ? memory[targetKey] as Entity : entity;
		int amount = amountKey != "" ? (int)memory[amountKey] : defaultAmount;

		if (target.health > 0 &&
			entity.position.ManhattanDistance(target.position) <= range) {
			// View
			entity.FaceTowards(target);
			entity.PlayAnimation(animationKey);

			switch (mode) {
				case Mode.Damage:
					target.Damage(amount);
					return target.health > 0 ? NodeStatus.Running : NodeStatus.Success;
				case Mode.Heal:
					target.Heal(amount);
					return target.health < target.maxHealth ? NodeStatus.Running : NodeStatus.Success;
				default:
					return NodeStatus.Failure;
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