using UnityEngine;

public class Leaf_MemoryTransaction : Behavior {

	[Header("Target Entity")]
	[Tooltip("Entity")]
	public string targetEntity;

	[Header("Read from me, write to target")]
	public string[] myReadKeys;
	public string[] targetWriteKeys;

	[Header("Read from target, write to me")]
	public string[] targetReadKeys;
	public string[] myWriteKeys;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		
		return NodeStatus.Success;
	}
}