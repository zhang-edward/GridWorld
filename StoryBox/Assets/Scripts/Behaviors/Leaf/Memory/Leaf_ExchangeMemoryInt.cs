using UnityEngine;
using System.Collections;

public class Leaf_ExchangeMemoryInt : Behavior {

	public enum Mode {
		Take,
		Give
	}

	public Mode mode;

	[Header("Target Entity")]
	public string entityKey;

	[Header("Read Keys")]
	public string amountKey;
	public int defaultAmount;

	[Header("Read and Write Keys")]
	public string myKey;
	public int defaultMine;
	[Space]
	public string otherKey;
	public int defaultOther;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(myKey, defaultMine);
		memory.SetDefault(amountKey, defaultAmount);

		Entity target = memory[entityKey] as Entity;
		target.memory.SetDefault(otherKey, defaultOther);

		int mine = (int)memory[myKey];
		int other = (int)target.memory[otherKey];
		int amount = (int)memory[amountKey];

		switch(mode) {
			case Mode.Take:
				target.memory[otherKey] = Mathf.Max(other - amount, 0);
				memory[myKey] = mine + Mathf.Min(amount, other);
				break;
			case Mode.Give:
				memory[myKey] = Mathf.Max(mine - amount, 0);
				target.memory[otherKey] = other + Mathf.Min(amount, mine);
				break;
		}
		return NodeStatus.Success;
	}
}
