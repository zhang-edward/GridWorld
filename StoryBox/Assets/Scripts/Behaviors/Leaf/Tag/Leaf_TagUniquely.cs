using UnityEngine;

public class Leaf_TagUniquely : Behavior {

	private enum Mode {
		Add,
		Delete,
	}

	[SerializeField] private Mode mode = Mode.Add;

	[Header("Read Keys")]
	public string entityKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity target = memory[entityKey] as Entity;
		switch (mode) {
			case Mode.Add:
				target.tags.Add(entity.uniqueTag);
				break;
			case Mode.Delete:
				if (target.tags.Contains(entity.uniqueTag))
					target.tags.Remove(entity.uniqueTag);
				else
					return NodeStatus.Failure;
				break;
		}
		return NodeStatus.Success;
	}
}