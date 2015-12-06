using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MovingEntity : Entity {
	
	private float moveUpChance;
	private float moveRightChance;

	// list of types of terrain this entity can move on
	public List<int> canMove = new List<int>();

	
	// vector2 that represents an unvisited cell
	public static Vector2 empty = new Vector2(-1, -1);
	
	public void AttemptMove(int destX, int destY)
	{
		if (world.isInBounds (destX, destY))
		{
			if (canMove.Contains(world.terrain[destY, destX]) &&
			    world.entities[destY, destX] == null)
			{
				//Debug.Log ("moved");
				world.entities[y, x] = null;
				x = destX;
				y = destY;
				world.entities[y, x] = this;
			}
		}
	}

	/// <summary>
	/// Randoms the move toward.
	/// </summary>
	/// <param name="destX">Destination x.</param>
	/// <param name="destY">Destination y.</param>
	/// <param name="attraction">Higher = more likely to move towards the target</param>
	public void RandomMoveToward(int destX, int destY, float attraction)
	{
		//Debug.Log ("BeginRMT");
		if (x < destX)
			moveRightChance = attraction;
		else
			moveRightChance = 1 - attraction;
		if (y < destY)
			moveUpChance = attraction;
		else
			moveUpChance = 1 - attraction;

		if (Random.value < 0.5)
		{
			if (Random.value < moveRightChance)
			{
				//Debug.Log ("AttemptMove");
				AttemptMove (x + 1, y);
			}
			else
			{
				//Debug.Log ("AttemptMove");
				AttemptMove (x - 1, y);
			}
		}
		else
		{
			if (Random.value < moveUpChance)
			{
				//Debug.Log ("AttemptMove");
				AttemptMove(x, y + 1);
			}
			else
			{
				//Debug.Log ("AttemptMove");
				AttemptMove(x, y - 1);
			}
		}
		//Debug.Log ("EndRMT");
	}
}
