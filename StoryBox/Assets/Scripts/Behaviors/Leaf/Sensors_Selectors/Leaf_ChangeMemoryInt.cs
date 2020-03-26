using UnityEngine;

public class Leaf_ChangeMemoryInt : Behavior {
#pragma warning disable CS0649
	private enum Mode {
		Add,
		Subtract,
		Set
	}

	[SerializeField] private Mode mode;
	public int value;
	public int defaultValue;

	[Header("Read and Write Keys")]
	public string key;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(key, defaultValue);
		if (mode == Mode.Add)
			memory[key] = (int) memory[key] + value;
		if (mode == Mode.Subtract)
			memory[key] = (int) memory[key] - value;
		if (mode == Mode.Set)
			memory[key] = value;

		return NodeStatus.Success;
	}
}