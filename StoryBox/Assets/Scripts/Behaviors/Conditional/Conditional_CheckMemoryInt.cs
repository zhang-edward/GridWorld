using UnityEngine;

public class Conditional_CheckMemoryInt : Behavior {

	private enum Mode {
		Equal,
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo
	}

	[SerializeField] Mode mode = Mode.Equal;
	public int value;
	public int defaultValue;

	[Header("Read Keys")]
	public string key;

	public override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(key, defaultValue);

		bool result = false;
		int mem = (int)memory[key];
		switch (mode) {
			case Mode.Equal:
				result = mem == value;
				break;
			case Mode.GreaterThan:
				result = mem > value;
				break;
			case Mode.LessThan:
				result = mem < value;
				break;
			case Mode.GreaterThanOrEqualTo:
				result = mem >= value;
				break;
			case Mode.LessThanOrEqualTo:
				result = mem <= value;
				break;
		}
		return result ? NodeStatus.Success : NodeStatus.Failure;
	}
}