using System.Collections.Generic;
using UnityEngine;

public class Leaf_PositionSelector : Behavior {

	private Vector2Int NONE = new Vector2Int(-1, -1);

	public enum Mode {
		Nearest,
		Random
	}
	public Mode mode;

	[Header("Read Keys")]
	[Tooltip("List<Vector2Int>")]
	public string positionsKey = "positions";

	[Header("Write Keys")]
	public string selectedKey = "selected";

	protected override NodeStatus Act(Entity entity, Memory memory) {
		List<Vector2Int> positions = memory[positionsKey] as List<Vector2Int>;
		if (positions.Count <= 0)
			return NodeStatus.Failure;
		positions.Shuffle();

		Vector2Int selected = NONE;
		switch (mode) {
			case Mode.Nearest:
				selected = SelectNearest(entity.position, positions);
				break;
			case Mode.Random:
				positions.Shuffle();
				selected = positions[0];
				break;
		}

		if (selected != NONE) {
			memory[selectedKey] = selected;
			return NodeStatus.Success;
		} else
			return NodeStatus.Failure;
	}

	private Vector2Int SelectNearest(Vector2Int myPos, List<Vector2Int> positions) {
		int minDist = int.MaxValue;
		Vector2Int selected = NONE;
		foreach (Vector2Int pos in positions) {
			int dist = pos.ManhattanDistance(myPos);
			if (dist < minDist) {
				minDist = dist;
				selected = pos;
			}
		}
		return selected;
	}
}