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
		if (i < behaviors.Length)
			return $"{gameObject.name} (Sequence) \n{behaviors[i].PrintTreeTraversal(stack)}";
		else
			return $"{gameObject.name} (Sequence) all succeeded";
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
		entity.currentNodes.Push(i);
		return status;
	}
}