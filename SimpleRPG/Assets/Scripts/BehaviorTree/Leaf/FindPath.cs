using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviors
{
	public class FindPath : Behavior {

		private Entity e;
		private Blackboard b;
		private int destX;
		private int destY;

		List<Vector2> path = new List<Vector2>();
		private Vector2 empty = new Vector2(-1, -1);

		public FindPath(Entity e, Blackboard b)
		{
			this.e = e;
			this.b = b;
		}

		public override void Reset()
		{
			b.setValue("Path", null);
		}

		public override NodeStatus Act()
		{
			destX = (int)b.getValue("TargetX");
			destY = (int)b.getValue("TargetY");

			// if getting path succeeds
			bool foundPath = TryGetPath();
			if (foundPath)
			{
				// DEBUG =========================
				/*foreach (Vector2 node in path)
				{
					Debug.Log(node);
				}*/
				// DEBUG =========================
				//Debug.Log("found path");
				b.setValue("Path", path);
				b.setValue("PathLength", path.Count);

				e.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);

				return NodeStatus.Success;
			}
			//Debug.Log("found path failed");
			e.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0f);
			return NodeStatus.Failure;
		}

		private bool TryGetPath()
		{
			path.Clear();
			List<Vector2> frontier = new List<Vector2>();
			Vector2[,] cameFrom = new Vector2[World.WORLD_SIZE, World.WORLD_SIZE];

			// initialize array with (-1, -1) to represent an unvisited cell
			for (int y = 0; y < World.WORLD_SIZE; y++)
				for (int x = 0; x < World.WORLD_SIZE; x++)
					cameFrom[y, x] = empty;

			frontier.Add(new Vector2(e.x, e.y));

			// check until no more frontier
			while (frontier.Count > 0)
			{
				// get the first cell in the frontier (and remove it)
				Vector2 current = frontier[0];
				frontier.RemoveAt(0);

				foreach (Vector2 neighbor in e.world.getAdjacentCoords((int)current.x, (int)current.y))
				{
					// neighbor x and y
					int nx = (int)neighbor.x;
					int ny = (int)neighbor.y;

					int neighborTerrain = e.world.terrain[ny, nx];
					// if the cell has not been visited yet
					// do not check unwalkable cells
					if ((e.canMove.Contains(neighborTerrain)) &&
						(cameFrom[ny, nx].Equals(empty)) &&
						(!e.world.EntityExistsAt(nx, ny)))
					{
						// store the cell coords that this neighbor CAME FROM
						cameFrom[ny, nx] = new Vector2(current.x, current.y);
						// add this cell to the frontier to be checked next time
						frontier.Add(new Vector2(nx, ny));

						if (nx == destX && ny == destY)
						{
							getPath(cameFrom, nx, ny);
							//Debug.Log("Path found. Path length: " + path.Count);
							return true;
						}
					}
				}
			}
			return false;
		}

		private void getPath(Vector2[,] cameFrom, int x, int y)
		{
			int pathX = x;
			int pathY = y;
			//Debug.Log("Pos: " + e.x + ", " + e.y);
			while (pathX != e.x || pathY != e.y)
			{
				path.Add(new Vector2(pathX, pathY));
				//Debug.Log(pathX + ", " + pathY);
				int tempX = (int)pathX;
				pathX = (int)cameFrom[pathY, pathX].x;
				pathY = (int)cameFrom[pathY, tempX].y;
			}
			path.Reverse();
		}
	}
}
