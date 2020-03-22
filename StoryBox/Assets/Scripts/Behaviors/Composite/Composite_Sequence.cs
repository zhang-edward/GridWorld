using UnityEngine;
public class Composite_Sequence : Behavior {

	public Behavior[] behaviors;

	public override void Init() {
		foreach (Behavior b in behaviors) {
			b.Init();
		}
	}

	public override string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack) {
		int i = stack.Pop();
		return $"{gameObject.name} (Sequence) \n{behaviors[i].PrintTreeTraversal(stack)}";
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	public override NodeStatus Act(Entity entity, Memory memory) {
		int i = entity.currentNodes.Count == 0 ? 0 : entity.currentNodes.Pop();
		NodeStatus status = NodeStatus.Failure;
		while (i < behaviors.Length) {
			// Run the current sub-behavior
			status = behaviors[i].Act(entity, memory);
			// Fails => continue to next one
			if (status == NodeStatus.Failure) {
				i = 0;
				break;
			}
			// Succeeds => break with status SUCCESS
			else if (status == NodeStatus.Success) {
				i++;
			}
			// Running => break with status RUNNING
			else if (status == NodeStatus.Running) {
				break;
			}
		}
		// If all sub-behaviors succeeded
		if (i == behaviors.Length)
			i = 0;
		entity.currentNodes.Push(i);
		return status;
	}
}