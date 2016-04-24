using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	/* IMPORTANT: in arrays "x" and "y" are flipped! so
	 * when getting a coordinate from an array you must use
	 * array[y, x] NOT array[x, y]
	 * */


	// Miner class for random "cave" formation generation
	// basically generates clumps of objects
	public class Miner
	{
		public int x = 0;
		public int y = 0;
		int xUpperBound;
		int xLowerBound;
		int yUpperBound;
		int yLowerBound;
		
		public Miner (int xUpBound, int xLowBound, int yUpBound, int yLowBound)
		{
			xUpperBound = xUpBound;
			xLowerBound = xLowBound;
			yUpperBound = yUpBound;
			yLowerBound = yLowBound;
		}
		
		public void moveRandomly()
		{
			int[] moves = {-1, 0, 1};
			x += moves[Random.Range (0, moves.Length)];
			y += moves[Random.Range (0, moves.Length)];
			// if miner moves out of bounds
			x = x > xLowerBound ? x : xLowerBound;
			x = x < xUpperBound ? x : xUpperBound;
			y = y > yLowerBound ? y : yLowerBound;
			y = y < yUpperBound ? y : yUpperBound;
		}

		public void setPosition(int xx, int yy)
		{
			x = xx;
			y = yy;
		}
		
		public void setRandomPosition()
		{
			x = Random.Range (xLowerBound, xUpperBound);
			y = Random.Range (yLowerBound, yUpperBound);
		}

		public int getXUpperBound()
		{
			return xUpperBound;
		}

		public int getXLowerBound()
		{
			return xLowerBound;
		}

		public int getYUpperBound()
		{
			return yUpperBound;
		}

		public int getYLowerBound()
		{
			return yLowerBound;
		}
	}	

	public GameObject[] entityPrefabs;				// list of possible Entities to be generated
	public GameObject[] terrainObjPrefabs;			// list of possible TerrainObjects to be generated
	public GameObject[] terrainStatusPrefabs;		// list of possible TerrainStatuses to be generated
	public GameObject[] terrainBiomePrefabs;		// list of possible TerrainBiomes to be generated

	/* Layers of Entities and Terrain Objects are generated individually
	 * */
	public List<Entity> entityLayer = new List<Entity>();
	public TerrainObject[,] terrainObjectLayer = new TerrainObject[WORLD_SIZE, WORLD_SIZE];
	public TerrainStatus[,] terrainStatusLayer = new TerrainStatus[WORLD_SIZE, WORLD_SIZE];
	public TerrainBiome[,] terrainBiomeLayer = new TerrainBiome[WORLD_SIZE, WORLD_SIZE];

	Transform entitiesFolder;
	Transform terrainObjectsFolder;
	Transform terrainStatusesFolder;
	Transform terrainBiomesFolder;

	public Player player;

	public const int WORLD_SIZE = 20;
	// public const Vector3 worldOffset = new Vector3(0, 0, 0);

	void Awake()
	{
		entitiesFolder = transform.FindChild ("Entities");
		terrainObjectsFolder = transform.FindChild ("TerrainObjects");
		terrainStatusesFolder = transform.FindChild ("TerrainStatuses");
		terrainBiomesFolder = transform.FindChild ("TerrainBiomes");
	}

	void Start()
	{
		CreateNewWorld();
	}

	void CreateNewWorld()
	{
		// Everything in this method is temporary and can be expanded upon
		LayoutMiner(250, terrainBiomeLayer, terrainBiomePrefabs[1]);

		for (int y = 0; y < WORLD_SIZE; y ++)
		{
			for (int x = 0; x < WORLD_SIZE; x ++)
			{
				if (terrainBiomeLayer[y, x] == null)
				{
					GameObject tile = Instantiate (terrainBiomePrefabs[2], new Vector3(x, y), Quaternion.identity) as GameObject;
					terrainBiomeLayer[y, x] = tile.GetComponent<TerrainBiome>();
				}
			}
		}

		LayoutMiner(18, terrainObjectLayer, terrainObjPrefabs[1]);
		LayoutMiner(10, terrainObjectLayer, terrainObjPrefabs[1]);
		LayoutMiner(12, terrainObjectLayer, terrainObjPrefabs[1]);
		LayoutMiner(15, terrainObjectLayer, terrainObjPrefabs[1]);

		LayoutMiner(3, terrainObjectLayer, terrainObjPrefabs[0]);
		LayoutMiner(4, terrainObjectLayer, terrainObjPrefabs[0]);

		PlacePlayer (entityPrefabs[0]);
		LayoutEntitiesScatter (10, entityLayer, entityPrefabs[1]);

		// Destroy objects not on land
		foreach (TerrainObject tObj in terrainObjectLayer)
		{
			if (tObj != null)
			{
				if (terrainBiomeLayer[tObj.y, tObj.x].tileName.Equals("waterTile"))
				{
					DestroyImmediate(tObj.gameObject);
				}
			}
		}

		foreach (Entity a in entityLayer)
			if (a != null)
				a.transform.SetParent (entitiesFolder);
		foreach (TerrainObject a in terrainObjectLayer)
			if (a != null)
				a.transform.SetParent (terrainObjectsFolder);
		foreach (TerrainStatus a in terrainStatusLayer)
			if (a != null)	
				a.transform.SetParent (terrainStatusesFolder);
		foreach (TerrainBiome a in terrainBiomeLayer)
			if (a != null)
				a.transform.SetParent (terrainBiomesFolder);
	}

	public void UpdateWorldLayers()
	{
	}

	void PlacePlayer(GameObject tilePrefab)
	{
		int xPos = Random.Range (0, WORLD_SIZE);
		int yPos = Random.Range (0, WORLD_SIZE);
		while (terrainBiomeLayer[yPos, xPos].tileName == "waterTile")
		{
			xPos = Random.Range (0, WORLD_SIZE);
			yPos = Random.Range (0, WORLD_SIZE);
		}
		player = createWorldObject<Player>(xPos, yPos, tilePrefab) as Player;
		player.Init(10, 5, 3);
		entityLayer.Add(player);
	}

	/* Layout an amount of tile on the game layer using the miner method
	 * 
	 * Params:
	 * int count - the number of objects to create
	 * T[,] layer - the layer of the world to create the tiles on
	 * GameObject[] tiles - array of possible tiles to create
	 * */
	void LayoutMiner<T>(int count, T[,] layer, GameObject tilePrefab)
		where T : WorldObject
	{
		// Instantiate a new miner and set it to a random position in the grid
		Miner miner = new Miner(WORLD_SIZE - 1, 0, WORLD_SIZE - 1, 0);
		miner.setRandomPosition();
		
		int i = count;
		while (i > 0)
		{
			miner.moveRandomly();
			int xPos = miner.x;
			int yPos = miner.y;
			
			// if there is nothing currently in the position
			if (layer[yPos, xPos] == null)
			{
				WorldObject worldObj = createWorldObject<T>(xPos, yPos, tilePrefab);
				layer[yPos, xPos] = (T)worldObj;
				
				// if a tile is spawned then decrement the counter
				i --;
			}
		}
	}


	void LayoutEntitiesMiner<T>(int count, List<T> layer, GameObject tilePrefab)
		where T : WorldObject
	{
		// Instantiate a new miner and set it to a random position in the grid
		Miner miner = new Miner(WORLD_SIZE - 1, 0, WORLD_SIZE - 1, 0);
		miner.setRandomPosition();
		
		int i = count;
		while (i > 0)
		{
			miner.moveRandomly();
			int xPos = miner.x;
			int yPos = miner.y;
			
			// if there is no entity already in that position
			if (checkPositionForEntity(xPos, yPos)  == null)
			{
				WorldObject worldObj = createWorldObject<T>(xPos, yPos, tilePrefab);
				layer.Add((T)worldObj);
				
				// if a tile is spawned then decrement the counter
				i --;
			}
		}
	}

	// layout objects on its matching layer randomly scattered
	void LayoutScatter<T>(int count, T[,] layer, GameObject tilePrefab)
		where T : WorldObject
	{
		int i = count;
		while (i > 0)
		{
			int xPos = Random.Range (0, WORLD_SIZE);
			int yPos = Random.Range (0, WORLD_SIZE);

			WorldObject worldObj = createWorldObject<T>(xPos, yPos, tilePrefab);
			layer[yPos, xPos] = (T)worldObj;
			
			// if a tile is spawned then decrement the counter
			i --;
		}
	}
	
	void LayoutEntitiesScatter<T>(int count, List<T> layer, GameObject tilePrefab)
		where T : WorldObject
	{
		int i = count;
		while (i > 0)
		{
			int xPos = Random.Range (0, WORLD_SIZE);
			int yPos = Random.Range (0, WORLD_SIZE);
			// if there is no entity already in that position
			if (checkPositionForEntity(xPos, yPos) == null)
			{
				WorldObject worldObj = createWorldObject<T>(xPos, yPos, tilePrefab);

				Entity entity = (Entity)worldObj;
				entity.Init(Random.Range (5, 10),
				            Random.Range (1, 4),
				            Random.Range (1, 2));

				layer.Add((T)worldObj);

				// if a tile is spawned then decrement the counter
				i --;
			}
		}
	}
	
	// Returns true if there is an entity at the coordinates x and y
	public Entity checkPositionForEntity(int x, int y)
	{
		foreach (Entity entity in entityLayer)
			if (x == entity.x && y == entity.y)
				return entity;
		return null;
	}

	// Instantiates prefab at x and y
	private WorldObject createWorldObject<T>(int x, int y, GameObject tilePrefab)
		where T : WorldObject
	{
		// IMPORTANT: This Vector3 only works if the board has no offset from the origin
		// The x and y position on the matrix just happens to be the x and y position in world space because there is no offset
		Vector3 instantiatePosition = new Vector3(x, y);					// make a Vector3 from the xPos and yPos
		
		GameObject tile = Instantiate (tilePrefab, instantiatePosition, Quaternion.identity) as GameObject;

		WorldObject worldObj = tile.GetComponent<T>();
		worldObj.InitCoords(x, y, this);

		return worldObj;
	}
}
