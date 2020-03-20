using UnityEngine;

public class Debug_IsNull : Behavior {

	[Header("Read Keys")]
	public string toPrintKey;

	public override NodeStatus Act(Entity entity, Memory memory) {
		Debug.Log(memory[toPrintKey] == null);
		return NodeStatus.Success;
	}
}