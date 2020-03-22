using UnityEngine;
using System.Collections;

public class Conditional_CompareMemoryInts : Behavior {
	private enum Mode {
		Equal,
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo
	}


	[Header("Read Keys")]
	public string key1;
	public int defaultValue1;
	[SerializeField] Mode mode = Mode.Equal;
	public string key2;
	public int defaultValue2;

	public override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(key1, defaultValue1);
		memory.SetDefault(key2, defaultValue2);

		bool result = false;
		int mem1 = (int)memory[key1];
		int mem2 = (int)memory[key2];
		switch (mode) {
			case Mode.Equal:
				result = mem1 == mem2;
				break;
			case Mode.GreaterThan:
				result = mem1 > mem2;
				break;
			case Mode.LessThan:
				result = mem1 < mem2;
				break;
			case Mode.GreaterThanOrEqualTo:
				result = mem1 >= mem2;
				break;
			case Mode.LessThanOrEqualTo:
				result = mem1 <= mem2;
				break;
		}
		return result ? NodeStatus.Success : NodeStatus.Failure;
	}
}
