using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaf_SetBehavior : Behavior {

	public GameObject[] behaviors;
	public int[] weights;

	[Header("Read Keys")]
	public string childEntitiesKey = "children";
	// [Header("Write Keys")]

	private float[] probs;

	public override void Init() {
		// Generate a probability distribution based on weights
		float sum = weights.Sum();
		probs = weights.Select(weight => weight / sum).ToArray();
	}

	public override NodeStatus Act(Entity entity, Memory memory) {
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
				// Assign behavior i to this entity
				GameObject obj = Instantiate(behaviors[i], e.transform);
				Behavior b = obj.GetComponent<Behavior>();
				e.AssignBehavior(b);
			}
		}
	}
}