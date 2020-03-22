using System.Collections.Generic;
using UnityEngine;

public class Leaf_PositionSelector : Behavior {

	private Vector2Int NONE = new Vector2Int(-1, -1);

	[Header("Read Keys")]
	[Tooltip("List<Vector2Int>")]
	public string positionsKey = "positions";

	[Header("Write Keys")]
	public string selectedKey = "selected";

	public override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> positions = memory[positionsKey] as List<Vector2Int>;
		positions.Shuffle();

		int minDist = int.MaxValue;
		Vector2Int selected = NONE;
		foreach (Vector2Int pos in positions) {
			int dist = pos.ManhattanDistance(entity.position);
			if (dist < minDist) {
				minDist = dist;
				selected = pos;
			}
		}
		if (selected != NONE) {
			memory[selectedKey] = selected;
			return NodeStatus.Success;
		} else
			return NodeStatus.Failure;
	}
}