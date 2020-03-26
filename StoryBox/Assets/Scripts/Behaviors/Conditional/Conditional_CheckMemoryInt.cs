using UnityEngine;

public class Conditional_CheckMemoryInt : Behavior {

	private enum Mode {
		Equal,
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo
	}


	[Header("Read Keys")]
	public string entityKey;
	public string key;
	[SerializeField] Mode mode = Mode.Equal;
	public int value;
	public int defaultValue;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Memory mem = entityKey != "" ? ((Entity)memory[entityKey]).memory : memory;
		mem.SetDefault(key, defaultValue);

		bool result = false;
		int memVal = (int)mem[key];
		switch (mode) {
			case Mode.Equal:
				result = memVal == value;
				break;
			case Mode.GreaterThan:
				result = memVal > value;
				break;
			case Mode.LessThan:
				result = memVal < value;
				break;
			case Mode.GreaterThanOrEqualTo:
				result = memVal >= value;
				break;
			case Mode.LessThanOrEqualTo:
				result = memVal <= value;
				break;
		}
		return result ? NodeStatus.Success : NodeStatus.Failure;
	}
}