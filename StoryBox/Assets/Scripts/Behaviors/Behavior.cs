using UnityEngine;

public abstract class Behavior : MonoBehaviour {

	public virtual void Init() { }

	public abstract NodeStatus Act(Entity entity, Memory memory);
}

public enum NodeStatus {
	Running,
	Failure,
	Success
}