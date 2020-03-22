using UnityEngine;
public class Composite_Sequence : Behavior {

	public Behavior[] behaviors;

	public override void Init() {
		foreach (Behavior b in behaviors) {
			b.Init();
		}
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	public override NodeStatus Act(Entity entity, Memory memory) {
		int i = entity.currentNodes.Count == 0 ? 0 : entity.currentNodes.Pop();
		NodeStatus returnStatus = NodeStatus.Failure;

		while (i < behaviors.Length) {
			// Run the current sub-behavior
			NodeStatus status = behaviors[i].Act(entity, memory);
			// Fails => continue to next one
			if (status == NodeStatus.Failure) {
				if (GameManager.instance.debuggingLog) Debug.Log(behaviors[i] + ": Failure", entity.gameObject);
				i = 0;
				returnStatus = NodeStatus.Failure;
				break;
			}
			// Succeeds => break with status SUCCESS
			else if (status == NodeStatus.Success) {
				if (GameManager.instance.debuggingLog) Debug.Log($"{behaviors[i]} success", entity.gameObject);
				i++;
			}
			// Running => break with status RUNNING
			else if (status == NodeStatus.Running) {
				if (GameManager.instance.debuggingLog) Debug.Log(behaviors[i] + ": Running", this);
				returnStatus = NodeStatus.Running;
				break;
			}
		}
		// If all sub-behaviors succeeded
		if (i == behaviors.Length) {
			i = 0;
			returnStatus = NodeStatus.Success;
		}
		entity.currentNodes.Push(i);
		return returnStatus;
	}
}