using UnityEngine;
using System.Collections;

public class Leaf_AnimateDelay : Behavior {

	public string animationKey;
	public int time;

	private string timerKey {
		get { return "timer: " + GetInstanceID(); }
	}
	private string isAnimating {
		get { return "is_animating: " + GetInstanceID(); }
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(timerKey, time);
		memory.SetDefault(isAnimating, false);

		// If the entity is not currently playing the animation
		if (!(bool)memory[isAnimating]) {
			// Play animation and set flag
			entity.PlayAnimation(animationKey);
			memory[isAnimating] = true;
		}

		// Timer logic
		int timer = (int)memory[timerKey];
		timer--;

		// Timer finished
		if (timer <= 0) {
			// Reset timer and save to memory
			timer = time;
			memory[timerKey] = timer;
			// Reset animating flag and save to memory
			memory[isAnimating] = false;
			// Reset animation
			entity.ResetAnimation();
			return NodeStatus.Success;
		}
		// Otherwise, save timer to memory for next tick
		else {
			memory[timerKey] = timer;
			return NodeStatus.Running;
		}
	}
}
