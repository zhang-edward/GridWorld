using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gatherer : ColonyEntity {

	public Sprite normalSprite;
	public Sprite carryingSprite;
	private SpriteRenderer sr;

	private Vector2 targetResource = empty;

	public bool carryingResource = false;

	//TODO: make it so the gatherer can get trees (id:4) or rocks(id:5)

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public override void Act ()
	{
		// if this gatherer is not carrying a resource, then move to a target resource
		if (!carryingResource)
		{
			sr.sprite = normalSprite;

			// find a resource and move to it
			if (targetResource.Equals(empty))
			{
				targetResource = findNearestForest();
				Debug.Log (targetResource);
			}
			else
			{
				// if there is a resource on a neighboring tile, then take that resource
				if (world.getNeighboringTerrain(x, y).Contains (4))
				{
					carryingResource = true;
				}
				else
				{
					int destX = (int)targetResource.x;
					int destY = (int)targetResource.y;
					//Debug.Log ("GathererMove");
					RandomMoveToward(destX, destY, 0.9f);
				}
			}
		}
		else
		{
			sr.sprite = carryingSprite;

			if (world.getNeighboringEntities(x, y).Contains (colonyCenter))
			{
				carryingResource = false;
				colonyCenter.trees ++;
			}
			else
			{
				RandomMoveToward (colonyCenter.x, colonyCenter.y, 0.9f);
			}
		}
		//Debug.Log ("EndAct");
	}



	// breadth-first search
	public Vector2 findNearestForest()
	{
		List<Vector2> frontier = new List<Vector2>();
		Vector2[,] cameFrom = new Vector2[World.WORLD_SIZE, World.WORLD_SIZE];

		// initialize array with (-1, -1) to represent an unvisited cell
		for(int y = 0; y < World.WORLD_SIZE; y ++)
			for (int x = 0; x < World.WORLD_SIZE; x ++)
				cameFrom[y, x] = empty;

		frontier.Add (new Vector2(this.x, this.y));

		// check until no more frontier
		while (frontier.Count > 0)
		{
			// get the first cell in the frontier (and remove it)
			Vector2 current = frontier[0];
			frontier.RemoveAt(0);

			// check each neighbor
			foreach (Vector2 neighbor in world.getAdjacentCoords((int)current.x, (int)current.y))
			{
				int neighborX = (int)neighbor.x;
				int neighborY = (int)neighbor.y;

				// if the cell has not been visited yet
				int neighborTerrain = world.terrain[neighborY, neighborX];
				//world.terrainSpriteMap[neighborY, neighborX].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
				if ((canMove.Contains(neighborTerrain) || neighborTerrain == 4) &&	// only check if the terrain is walkable or a forest
					cameFrom[neighborY, neighborX].Equals(new Vector2(-1, -1)))
				{
					// store the cell coords that this neighbor CAME FROM
					cameFrom[neighborY, neighborX] = new Vector2(current.x, current.y);
					// add this cell to the frontier to be checked next time
					frontier.Add (new Vector2(neighborX, neighborY));

					// if we found a forest, we are done
					if (world.terrain[neighborY, neighborX] == 4)
					{
						return new Vector2(neighborX, neighborY);
					}
				}
			}
		}
		// if no forests found
		return empty;
	}
}
