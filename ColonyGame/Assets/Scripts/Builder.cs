using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Builder : ColonyEntity {

	public override void Act ()
	{
		// if standing adjacent to the frontier of territory
		if (territory.checkTerritory(x, y) != colonyCenter.Faction &&
			territory.checkSurroundingTerritory(colonyCenter.Faction, x, y))
		{
			colonyCenter.CreateColonyBuilding(0, x, y);
			gameObject.SetActive (false);
		}
		else
		{
			// wander randomly
			RandomMoveToward(x, y, 0.5f);
		}
	}
}
