using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf_MemoryIntOperation : Behavior {
	public enum Mode {
		Add,
		Subtract
	}

	[Header("Read Keys")]
	public string key1;
	public Mode mode;
	public string key2;

	[Header("Write Keys")]
	public string writeKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		int num1 = (int)memory[key1];
		int num2 = (int)memory[key2];
		int result = 0;

		switch(mode) {
			case Mode.Add:
				result = num1 + num2;
				break;
			case Mode.Subtract:
				result = num1 - num2;
				break;
		}

		memory[writeKey] = result;
		return NodeStatus.Success;
	}
}
