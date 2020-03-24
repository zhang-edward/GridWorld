using UnityEngine;

public class Conditional_Cooldown : Behavior {

	public int cooldown = 10;
	public int initialTimer = 1;

	private int timer;

	public override void Init() {
		base.Init();
		timer = initialTimer;
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		timer--;
		if (timer <= 0) {
			timer = cooldown;
			return NodeStatus.Success;
		} else
			return NodeStatus.Failure;
	}
}