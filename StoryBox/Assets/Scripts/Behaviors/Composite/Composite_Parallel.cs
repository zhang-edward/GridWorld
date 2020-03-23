using UnityEngine;

public class Composite_Parallel : Behavior {

	public Behavior[] behaviors;
	private int maskAll;

	public override void Init() {
		if (behaviors.Length >= 32) {
			Debug.LogError("Too many sub-behaviors!");
		}
		maskAll = (behaviors.Length << 1) - 1;
		foreach (Behavior b in behaviors) {
			b.Init();
		}
	}

	public override string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack, Entity entity) {
		string ans = "";
		int mask = stack.Pop();
		for (int i = 0; i < behaviors.Length; i++) {
			int curMask = i << 1;
			if ((mask & curMask) == 0) continue;
			ans += $"{behaviors[i].PrintTreeTraversal(stack, entity)}\n";
		}
		return $"{gameObject.name} (Priority):\n=={ans}==";
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	public override NodeStatus Act(Entity entity, Memory memory) {
		// The value stored on the stack is a bitmask indicating which tasks should be run and which ones are done
		int mask = entity.currentNodes.Count == 0 ? maskAll : entity.currentNodes.Pop();
		int returnMask = 0;
		bool subBehaviorRunning = false;

		NodeStatus status = NodeStatus.Failure;
		for (int i = 0; i < behaviors.Length; i ++) {
			// Check mask to see if this sub-behavior is still active
			int curMask = i == 0 ? 1 : i << 1;
			if ((mask & curMask) == 0) continue;

			// Run the current sub-behavior
			status = behaviors[i].Act(entity, memory);
			print($"{behaviors[i]}: {status.ToString()}");
			// print($"{behaviors[i]}: {status.ToString()}");
			// Fails => break with status FAILURE
			if (status == NodeStatus.Failure) {
				entity.currentNodes.Clear(); // Any downstream tree traversal is now wrong
				entity.currentNodes.Push(maskAll);
				return NodeStatus.Failure;
			}
			// Running => continue
			else if (status == NodeStatus.Running) {
				returnMask |= curMask;
				subBehaviorRunning = true;
			}
		}
		// If no subBehavior running, then we made it through all nodes with success code
		if (subBehaviorRunning) {
			entity.currentNodes.Push(returnMask);
			return NodeStatus.Running;
		}
		else {
			entity.currentNodes.Push(maskAll);
			return NodeStatus.Success;
		}
	}
}