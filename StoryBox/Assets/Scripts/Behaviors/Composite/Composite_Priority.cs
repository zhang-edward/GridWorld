using UnityEngine;

public class Composite_Priority : Behavior {

	public Behavior[] behaviors;

	public override void Init() {
		foreach (Behavior b in behaviors) {
			b.Init();
		}
	}

	public override string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack, Entity entity) {
		int i = stack.Pop();
		if (i < behaviors.Length)
			return $"{gameObject.name} (Priority) \n{behaviors[i].PrintTreeTraversal(stack, entity)}";
		else
			return $"{gameObject.name} (Priority) all failed";
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	public override NodeStatus Act(Entity entity, Memory memory) {
		int i = entity.currentNodes.Count == 0 ? 0 : entity.currentNodes.Pop();
		// When we run through all the behaviors, i = behaviors.length is saved on the stack for 
		// infomation-preserving purposes.
		i = i % behaviors.Length;

		NodeStatus status = NodeStatus.Failure;
		while (i < behaviors.Length) {
			// Run the current sub-behavior
			status = behaviors[i].Act(entity, memory);
			// print($"{behaviors[i]}: {status.ToString()}");
			// Fails => continue to next one
			if (status == NodeStatus.Failure) {
				entity.currentNodes.Clear(); // Any downstream tree traversal is now wrong
				i++;
			}
			// Succeeds => break with status SUCCESS
			else if (status == NodeStatus.Success) {
				entity.currentNodes.Clear(); // Any downstream tree traversal is now wrong
				i = 0;
				break;
			}
			// Running => break with status RUNNING
			else if (status == NodeStatus.Running) {
				break;
			}
		}
		entity.currentNodes.Push(i);
		return status;
	}
}