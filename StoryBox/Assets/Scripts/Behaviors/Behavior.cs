using UnityEngine;

public abstract class Behavior : MonoBehaviour {

	protected Entity entity;
	protected Memory memory;

	public virtual void Init(Entity entity, Memory memory) {
		this.entity = entity;
		this.memory = memory;
	}

	public abstract NodeStatus Act();
}

public enum NodeStatus {
	Running,
	Failure,
	Success
}