using UnityEngine;

public class Leaf_Delay : Behavior {

	public int time = 1;
	private int timer;

	public override void Init() {
		base.Init();
		timer = time;
	}

	public override NodeStatus Act(Entity entity, Memory memory) {
		timer--;
		if (timer <= 0) {
			timer = time;
			return NodeStatus.Success;
		} else {
			return NodeStatus.Running;
		}
	}
}