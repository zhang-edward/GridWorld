using UnityEngine;

public class Leaf_ModifyTag : Behavior {

	private enum Mode {
		Add,
		Delete,
	}

	[SerializeField] private Mode mode = Mode.Add;
	public string tagValue;

	[Header("Read Keys")]
	public string entityKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = memory[entityKey] as Entity;
		switch (mode) {
			case Mode.Add:
				if (target.tags.Contains(tagValue))
					return NodeStatus.Failure;
				else
					target.tags.Add(tagValue);
				break;
			case Mode.Delete:
				if (target.tags.Contains(tagValue))
					target.tags.Remove(tagValue);
				else
					return NodeStatus.Failure;
				break;
		}
		return NodeStatus.Success;
	}
}