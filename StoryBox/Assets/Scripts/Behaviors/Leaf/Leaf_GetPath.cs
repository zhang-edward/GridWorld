using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class Leaf_GetPath : Behavior {

	// [Tooltip("String that contains what terrain is movable for this entity")]
	public List<int> moveable;

	[Header("Read Keys")]
	public string destinationKey = "destination";

	[Header("Write Keys")]
	public string pathKey = "path";

	private Vector2Int EMPTY = new Vector2Int(-1, -1);
	private int[, ] baseMap;

	public override void Init(Entity entity, Memory memory) {
		base.Init(entity, memory);
		this.baseMap = entity.world.BaseMap;
	}

	public override NodeStatus Act() {
		// HashSet<int> moveable = memory[moveableKey] as HashSet<int>;
		Vector2Int destination = (Vector2Int) memory[destinationKey];
		Stack<Vector2Int> path = TryGetPath(entity.position.x, entity.position.y, destination.x, destination.y);
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
	public Stack<Vector2Int> TryGetPath(int startX, int startY, int destX, int destY) {
		print($"Getting position from {startX}, {startY} to {destX}, {destY}");
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

			List<Vector2Int> unweightedNeighbors = GetAdjacentCoords(current.x, current.y);
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
					return GetHalfPath(cameFrom, nx, ny);
				}

				int neighborTerrain = baseMap[ny, nx];
				// if the cell has not been visited yet, is walkable, and doesn't contain another entity
				if ((moveable.Contains(neighborTerrain)) &&
					(cameFrom[ny, nx].Equals(EMPTY))) { //&&
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
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	private Stack<Vector2Int> GetHalfPath(Vector2Int[, ] cameFrom, int x, int y) {
		Stack<Vector2Int> ans = new Stack<Vector2Int>();
		List<Vector2Int> fullPath = new List<Vector2Int>();
		int pathX = x;
		int pathY = y;
		while (pathX != entity.position.x || pathY != entity.position.y) {
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

	private List<Vector2Int> GetAdjacentCoords(int x, int y) {
		List<Vector2Int> ans = new List<Vector2Int>();
		int xx, yy;

		xx = x + 1;
		yy = y;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x;
		yy = y + 1;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x - 1;
		yy = y;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x;
		yy = y - 1;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		return ans;
	}

	private bool InBounds(int x, int y) {
		return x >= 0 && x < World.WORLD_SIZE && y >= 0 && y < World.WORLD_SIZE;
	}
}