using UnityEngine;
using System.Collections;

public class Enemy : Entity {

	public const float WANDER_CHANCE = 0.50f;

	void Awake () 
	{

	}

	public override void Act ()
	{
		// wander 1 square up, down, left, or right
		if (Random.value < WANDER_CHANCE)
		{
			// this Random.Range(0, 4) picks from 0 - 3
			int rand = Random.Range(0, 4);
			if (rand == 0)
				AttemptMove (new Vector3(x - 1, y));
			else if (rand == 1)
				AttemptMove (new Vector3(x + 1, y));
			else if (rand == 2)
				AttemptMove (new Vector3(x, y - 1));
			else if (rand == 3)
				AttemptMove (new Vector3(x, y + 1));
		}
	}

	public void AttemptMove(Vector3 dest)
	{
		// TODO: display a message describing why the player cannot move
		
		int destX = (int)dest.x;
		int destY = (int)dest.y;
		
		// Makes sure entity does not move off of map or into water
		if (!isInBounds(destX, destY))
			return;
		if (world.terrainBiomeLayer[destY, destX].tileName == "waterTile")
			return;
		else
		{
			x = destX;
			y = destY;

			UpdatePos ();
		}
	}

}
