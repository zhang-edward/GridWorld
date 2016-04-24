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

	public GameObject[] colonyEntityPrefabs;
	public GameObject[] colonyBuildingPrefabs;

	public TerritoryMngr territory;

	public override void Act ()
	{
		if (trees >= 20)
		{
			CreateColonyEntity(1);
			trees -= 20;
		}
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
		// create a gatherer
		CreateColonyEntity(0);
	}

	public void CreateColonyEntity(int id)
	{
		// get a random spawnPos
		int i = Random.Range (0, spawnPoints.Count);
		Vector2 pos = spawnPoints[i];

		int x = (int)pos.x;
		int y = (int)pos.y;

		if (world.entities[y, x] == null)
		{
			ColonyEntity e = world.CreateEntity(colonyEntityPrefabs[id], x, y) as ColonyEntity;
			e.Init (this, this.territory);
		}
	}

	public void CreateColonyBuilding(int id, int x, int y)
	{
		ColonyBuilding e = world.CreateEntity(colonyBuildingPrefabs[id], x, y) as ColonyBuilding;
		e.Init (this, this.territory);
		territory.setSurroundingTerritory(faction, x, y);
	}
}
