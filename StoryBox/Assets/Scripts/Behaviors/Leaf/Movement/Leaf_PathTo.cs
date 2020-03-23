using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Combines GetPath and MovePath into one
/// </summary>
public class Leaf_PathTo : Behavior {

	private static Vector2Int EMPTY = new Vector2Int(-1, -1);
	public Behavior movement;

	[Header("Read Keys")]
	public string destinationKey = "destination";

	[Header("Write Keys")]
	public string movementKey = "move_dest";

	private string pathCacheKey {
		get { return "path_cache: " + GetInstanceID(); }
	}

	public override NodeStatus Act(Entity entity, Memory memory) {
		// Gets the destination from memory
		Vector2Int destination = ReadDestinationFromMemory(memory);
		if (destination == EMPTY) {
			return NodeStatus.Failure;
		}

		// If we're at our destination, we're DONE
		if (entity.position == destination) {
			memory[pathCacheKey] = null; // discard path in cache
			return NodeStatus.Success;
		}

		// Else, we must get a path
		// Gets a path from memory, if it exists. Otherwise, calculate the path
		Stack<Vector2Int> path = GetPath(entity, memory, destination);
		if (path == null) {
			return NodeStatus.Failure;
		}

		Vector2Int dest = path.Pop();
		memory[movementKey] = dest;

		switch (movement.Act(entity, memory)) {
			// Move was successful
			case NodeStatus.Success:
				memory[pathCacheKey] = path; // Save path for next tick
				return NodeStatus.Running;
				// Movement behavior is somehow multi-step
			case NodeStatus.Running:
				memory[pathCacheKey] = path; // Save path for next tick
				return NodeStatus.Running;
				// Movement behavior failed
			case NodeStatus.Failure:
				memory[pathCacheKey] = null; // discard path in cache
				return NodeStatus.Failure;
			default:
				return NodeStatus.Failure;
		}
	}

	private Vector2Int ReadDestinationFromMemory(Memory memory) {
		object obj = memory[destinationKey];
		Vector2Int destination;
		if (obj is Entity) {
			destination = ((Entity) obj).position;
		} else if (obj is Vector2Int) {
			destination = (Vector2Int) obj;
		} else {
			Debug.LogError($"Incompatible data type at {destinationKey}!", this);
			return EMPTY;
		}
		return destination;
	}

	private Stack<Vector2Int> GetPath(Entity entity, Memory memory, Vector2Int destination) {
		Stack<Vector2Int> path = memory[pathCacheKey] as Stack<Vector2Int>;
		path = path != null && path.Count > 0 ? path : Leaf_GetPath.TryGetPath(entity.world.BaseMap, entity.allowedTiles, entity.position.x, entity.position.y, destination.x, destination.y);
		return path;
	}
}