using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaf_SetBehavior : Behavior {

	public string[] behaviors;
	public int[] weights;

	[Header("Read Keys")]
	public string childEntitiesKey = "children";

	private float[] probs;

	public override void Init() {
		// Generate a probability distribution based on weights
		float sum = weights.Sum();
		probs = weights.Select(weight => weight / sum).ToArray();
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		List<Entity> entities = memory[childEntitiesKey] as List<Entity>;
		foreach (Entity e in entities)
			Assign(e);

		return NodeStatus.Success;
	}

	private void Assign(Entity e) {
		float rand = Random.value;
		float total = 0f;
		for (int i = 0; i < weights.Length; i++) {
			total += weights[i];
			if (rand < total) {
				e.AssignBehavior(behaviors[i]);
				return;
			}
		}
	}
}