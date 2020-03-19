using UnityEngine;

public class Composite_Sequence : Behavior {

	public Behavior[] behaviors;
	int i = 0; // current behavior that is running

	public override void Init(Entity entity, Memory memory) {
		foreach (Behavior b in behaviors) {
			b.Init(entity, memory);
		}
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	public override NodeStatus Act() {
		while (i < behaviors.Length) {
			switch (behaviors[i].Act()) {
				case NodeStatus.Failure:
					Debug.Log(behaviors[i] + " Failure", this);
					return NodeStatus.Failure;
				case NodeStatus.Success:
					Debug.Log($"{behaviors[i]} Success + continuing", this);
					i++;
					continue;
				case NodeStatus.Running:
					Debug.Log(behaviors[i] + " Running", this);
					return NodeStatus.Running;
			}
		}
		// done with all behaviors and all success
		i = 0;
		return NodeStatus.Success;
	}
}