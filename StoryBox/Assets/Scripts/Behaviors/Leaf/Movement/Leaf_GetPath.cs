using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class Leaf_GetPath : Behavior {
	
	private static Vector2Int EMPTY = new Vector2Int(-1, -1);

	[Header("Read Keys")]
	[Tooltip("Can be an Entity or a Vector2Int")]
	public string destinationKey = "destination";

	[Header("Write Keys")]
	[Tooltip("Stack<Vector2Int>")]
	public string pathKey = "path";


	protected override NodeStatus Act(Entity entity, Memory memory) {
		int[, ] baseMap = entity.world.BaseMap;
		object obj = memory[destinationKey];
		Vector2Int destination;
		if (obj is Entity) {
			destination = ((Entity) obj).position;
		} else if (obj is Vector2Int) {
			destination = (Vector2Int) obj;
		} else {
			Debug.LogError($"Incompatible data type at {destinationKey}!", this);
			return NodeStatus.Failure;
		}

		Stack<Vector2Int> path = TryGetPath(baseMap, entity.allowedTiles, entity.position.x, entity.position.y, destination.x, destination.y);
		if (path == null) {
			return NodeStatus.Failure;
		} else {
			memory[pathKey] = path;
			return NodeStatus.Success;
		}
	}

	/// <summary>
	/// Implementation of Djikstra's algorithm
	/// </summary>
	/// <param name="startX">Starting X coordinate</param>
	/// <param name="startY">Starting Y coordinate</param>
	/// <param name="destX">Destination X coordinate</param>
	/// <param name="destY">Destination Y coordinate</param>
	/// <returns></returns>
	public static Stack<Vector2Int> TryGetPath(int[, ] baseMap, List<int> allowedTiles, int startX, int startY, int destX, int destY) {
		// print($"Getting position from {startX}, {startY} to {destX}, {destY}");
		// Get path
		List<Vector2Int> frontier = new List<Vector2Int>();
		Vector2Int[, ] cameFrom = new Vector2Int[World.WORLD_SIZE, World.WORLD_SIZE];

		// initialize array with (-1, -1) to represent an unvisited cell
		for (int y = 0; y < World.WORLD_SIZE; y++)
			for (int x = 0; x < World.WORLD_SIZE; x++)
				cameFrom[y, x] = EMPTY;

		frontier.Add(new Vector2Int(startX, startY));

		// check until no more frontier
		while (frontier.Count > 0) {
			// get the first cell in the frontier (and remove it)
			Vector2Int current = frontier[0];
			frontier.RemoveAt(0);

			List<Vector2Int> unweightedNeighbors = World.GetAdjacentCoords(current.x, current.y);
			SimplePriorityQueue<Vector2Int, float> neighbors = new SimplePriorityQueue<Vector2Int, float>();
			foreach (Vector2Int neighbor in unweightedNeighbors) {
				neighbors.Enqueue(neighbor, -Vector2Int.Distance(neighbor, new Vector2Int(destX, destY)) + Random.Range(-0.5f, 0.5f));
			}

			while (neighbors.Count > 0) {
				Vector2Int neighbor = neighbors.Dequeue();
				// neighbor x and y
				int nx = neighbor.x;
				int ny = neighbor.y;

				if (nx == destX && ny == destY) {
					cameFrom[ny, nx] = new Vector2Int(current.x, current.y);
					return GetHalfPath(cameFrom, startX, startY, nx, ny);
				}

				int neighborTerrain = baseMap[ny, nx];
				// if the cell has not been visited yet, is walkable, and doesn't contain another entity
				if (allowedTiles.Contains(neighborTerrain) &&
					cameFrom[ny, nx].Equals(EMPTY) &&
					!EntityManager.instance.CapacityReachedAt(nx, ny)) {
					// (!entity.world.EntityExistsAt(nx, ny))) {
					// store the cell coords that this neighbor CAME FROM
					cameFrom[ny, nx] = new Vector2Int(current.x, current.y);
					// add this cell to the frontier to be checked next time
					frontier.Add(new Vector2Int(nx, ny));
				}
			}
		}
		return null;
	}

	/// <summary>
	/// Gets half of the path to the target, since want want to recalculate the path once we traversed
	/// half of it.
	/// </summary>
	/// <param name="cameFrom"></param>
	/// <param name="destX"></param>
	/// <param name="destY"></param>
	/// <returns></returns>
	private static Stack<Vector2Int> GetHalfPath(Vector2Int[, ] cameFrom, int startX, int startY, int destX, int destY) {
		Stack<Vector2Int> ans = new Stack<Vector2Int>();
		List<Vector2Int> fullPath = new List<Vector2Int>();
		int pathX = destX;
		int pathY = destY;
		while (pathX != startX || pathY != startY) {
			fullPath.Add(new Vector2Int(pathX, pathY));
			int tempX = pathX;
			pathX = cameFrom[pathY, pathX].x;
			pathY = cameFrom[pathY, tempX].y;
		}
		// fullPath[0] = end of path
		// fullPath[fullPath.Count - 1] = start of path
		for (int i = (fullPath.Count / 2); i < fullPath.Count; i++) {
			ans.Push(fullPath[i]);
		}
		return ans;
	}
}