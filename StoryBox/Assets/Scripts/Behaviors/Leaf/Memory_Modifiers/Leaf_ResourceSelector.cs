using System.Collections.Generic;
using UnityEngine;

public class Leaf_ResourceSelector : Behavior {

	private Vector2Int NONE = new Vector2Int(-1, -1);

	[Header("Read Keys")]
	public string structuresKey = "structures";

	[Header("Write Keys")]
	public string selectedKey = "selected_structure";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> structs = memory[structuresKey] as List<Vector2Int>;
		int minDist = int.MaxValue;
		Vector2Int selected = NONE;
		foreach (Vector2Int vec in structs) {
			int dist = vec.ManhattanDistance(entity.position);
			if (dist < minDist) {
				minDist = dist;
				selected = vec;
			}
		}
		if (selected != NONE) {
			memory[selectedKey] = selected;
			return NodeStatus.Success;
		}
		else
			return NodeStatus.Failure;
	}
}