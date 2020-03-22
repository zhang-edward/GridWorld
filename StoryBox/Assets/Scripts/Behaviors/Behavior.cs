using UnityEngine;

public abstract class Behavior : MonoBehaviour {

	public virtual void Init() { }
	/// <summary>
	/// Used for debugging purposes, prints the traversal of the behavior tree
	/// </summary>
	/// <param name="stack">The child nodes of each composite node in the BT traversal</param>
	/// <returns></returns>
	public virtual string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack) { return gameObject.name; }
	public abstract NodeStatus Act(Entity entity, Memory memory);
}

public enum NodeStatus {
	Running,
	Failure,
	Success
}