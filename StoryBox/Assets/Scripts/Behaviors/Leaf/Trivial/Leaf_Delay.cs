using UnityEngine;

public class Leaf_Delay : Behavior {

	public int time;

	[Header("Read and Write Keys")]
	public string timerKey = "timer";

	public override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(timerKey, time);

		int timer = (int) memory[timerKey];
		timer--;
		if (timer <= 0) {
			timer = time;
			memory[timerKey] = timer;
			return NodeStatus.Success;
		} 
		else {
			memory[timerKey] = timer;
			return NodeStatus.Running;
		}
	}
}