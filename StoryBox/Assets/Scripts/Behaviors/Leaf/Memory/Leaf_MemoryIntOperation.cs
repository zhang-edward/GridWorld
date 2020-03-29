using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf_MemoryIntOperation : Behavior {
	public enum Mode {
		Add,
		Subtract,
		Set,
		Multiply
	}

	[Header("Read Keys")]
	public string key1;
	public int defaultValue1;
	public Mode mode;
	public string key2;
	public int defaultValue2;

	[Header("Write Keys")]
	public string writeKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(key1, defaultValue1);
		if (key2 != "")
			memory.SetDefault(key2, defaultValue2);

		int num1 = (int)memory[key1];
		int num2 = key2 != "" ? (int)memory[key2] : defaultValue2;
		int result = 0;

		switch(mode) {
			case Mode.Add:
				result = num1 + num2;
				break;
			case Mode.Subtract:
				result = num1 - num2;
				break;
			case Mode.Set:
				result = num2;
				break;
			case Mode.Multiply:
				result = num1 * num2;
				break;
		}

		memory[writeKey] = result;
		return NodeStatus.Success;
	}
}
