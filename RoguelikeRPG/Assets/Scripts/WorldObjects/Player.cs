using UnityEngine;
using System.Collections;

public class Player : Entity {

	/*[Header("Stats")]
	public int intelligence;
	public int strength;
	public int dexterity;*/

	void Start()
	{
	}

	void Update()
	{
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

		// check for another Entity at destination
		Entity targetEntity = world.checkPositionForEntity(destX, destY);
		if (targetEntity != null)
		{
			int dmgDealt = this.attack - targetEntity.defense;
			int dmgTaken = targetEntity.attack - this.defense;

			if (dmgDealt > 0)
				targetEntity.health -= dmgDealt;
			if (dmgTaken > 0)
				this.health -= dmgTaken;
		}
		else
		{
			x = destX;
			y = destY;
			
			UpdatePos();
		}
	}

	public override void Act () {Debug.LogError ("Player should not be calling Act method!");}
}
