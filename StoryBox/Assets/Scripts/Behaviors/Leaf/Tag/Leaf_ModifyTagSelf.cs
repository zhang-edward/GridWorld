using UnityEngine;

public class Leaf_ModifyTagSelf : Behavior {

	private enum Mode {
		Add,
		Delete,
	}

	[SerializeField] private Mode mode = Mode.Add;
	public string tagValue;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		switch (mode) {
			case Mode.Add:
				if (entity.tags.Contains(tagValue))
					return NodeStatus.Failure;
				else
					entity.tags.Add(tagValue);
				break;
			case Mode.Delete:
				if (entity.tags.Contains(tagValue))
					entity.tags.Remove(tagValue);
				else
					return NodeStatus.Failure;
				break;
		}
		return NodeStatus.Success;
	}
}