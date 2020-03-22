using UnityEngine;

public class Composite_Priority : Behavior {

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
				// if (GameManager.instance.debuggingLog) Debug.Log(behaviors[i] + " Failure", entity.gameObject);
				i++;
			}
			// Succeeds => break with status SUCCESS
			else if (status == NodeStatus.Success) {
				if (GameManager.instance.debuggingLog)
					Debug.Log($"{entity.gameObject.name}: {behaviors[i].gameObject.name}: Success", entity.gameObject);
				i = 0;
				returnStatus = NodeStatus.Success;
				break;
			}
			// Running => break with status RUNNING
			else if (status == NodeStatus.Running) {
				if (GameManager.instance.debuggingLog)
					Debug.Log($"{entity.gameObject.name}: {behaviors[i].gameObject.name}: Running", entity.gameObject);
				returnStatus = NodeStatus.Running;
				break;
			}
		}
		// If all sub-behaviors failed
		if (i == behaviors.Length) {
			i = 0;
			returnStatus = NodeStatus.Failure;
		}
		entity.currentNodes.Push(i);
		return returnStatus;
	}
}