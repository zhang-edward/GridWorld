using UnityEngine;

public class Leaf_ChangeMemoryInt : Behavior {
#pragma warning disable CS0649
	private enum Mode {
		Add,
		Subtract,
		Set
	}

	[Header("Read Keys")]
	public string entityKey;

	[Header("Read and Write Keys")]
	public string key;
	public int defaultValue;
	[SerializeField] private Mode mode;
	public int value;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Memory mem = entityKey != "" ? ((Entity)memory[entityKey]).memory : memory;

		mem.SetDefault(key, defaultValue);
		if (mode == Mode.Add)
			mem[key] = (int) mem[key] + value;
		if (mode == Mode.Subtract)
			mem[key] = (int) mem[key] - value;
		if (mode == Mode.Set)
			mem[key] = value;

		return NodeStatus.Success;
	}
}