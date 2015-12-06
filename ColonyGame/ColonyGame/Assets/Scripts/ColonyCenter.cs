using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColonyCenter : Entity {

	public int trees = 10;
	public int rocks = 5;
	List<Vector2> spawnPoints;

	private int faction;
	public int Faction{
		get{return faction;}
	}

	public TerritoryMngr territory;

	public override void Act ()
	{

	}

	public void Init(int faction)
	{
		this.faction = faction;
		territory.setSurroundingTerritory(faction, x, y);

		// set spawn points
		spawnPoints = world.getAdjacentCoords(x, y);
		foreach (Vector2 pos in spawnPoints)
		{
			// remove any spawn points that are over water
			if (world.terrain[(int)pos.y, (int)pos.x] == 0)
				spawnPoints.Remove (pos);
		}

		// TODO: put this in a method

	}

	public void CreateColonyEntity()
	{
		int randPos = Random.Range (0, spawnPoints.Count);
		Vector2 spawnPos = spawnPoints[randPos];
		if (world.entities[(int)spawnPos.y, (int)spawnPos.x] == null)
		{
			ColonyEntity e = world.CreateEntity(1, (int)spawnPos.x, (int)spawnPos.y) as ColonyEntity;
			e.Init (this, this.territory);
		}
	}
}
