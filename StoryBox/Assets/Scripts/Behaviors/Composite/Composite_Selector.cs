using UnityEngine;

public class Composite_Selector : Behavior {

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
					// Debug.Log(behaviors[i] + ": Failure", this);
					i++;
					continue;
				case NodeStatus.Success:
					// Debug.Log($"{behaviors[i]} success + continuing", this);
					i = 0;
					return NodeStatus.Success;
				case NodeStatus.Running:
					// Debug.Log(behaviors[i] + ": Running", this);
					return NodeStatus.Running;
			}
		}
		// done with all behaviors (all failure)
		i = 0;
		return NodeStatus.Failure;
	}
}