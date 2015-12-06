using UnityEngine;
using System.Collections;

public class TerritoryMngr : MonoBehaviour {

	private static int WORLD_SIZE = World.WORLD_SIZE;
	public World world;

	public SpriteRenderer[,] spriteMap = new SpriteRenderer[WORLD_SIZE, WORLD_SIZE];
	public int[,] territories = new int[WORLD_SIZE, WORLD_SIZE];

	public Sprite[] territorySprites;
	public GameObject prefab;

	void Start()
	{
		world = GetComponent<World>();
		Init();
	}

	public void Init()
	{
		for(int y = 0; y < WORLD_SIZE; y ++)
		{
			for (int x = 0; x < WORLD_SIZE; x ++)
			{
				GameObject o = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
				o.transform.SetParent(this.transform);
				o.transform.localPosition = new Vector2(x, y);

				spriteMap[y, x] = o.GetComponent<SpriteRenderer>();
				o.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);

				// -1 represents no territory
				territories[y, x] = -1;
			}
		}
	}

	void Update()
	{
		for(int y = 0; y < WORLD_SIZE; y ++)
		{
			for (int x = 0; x < WORLD_SIZE; x ++)
			{
				int spriteID = territories[y, x];
				if (spriteID != -1)
					spriteMap[y, x].sprite = territorySprites[spriteID];
			}
		}
	}

	public void setSurroundingTerritory(int faction, int x, int y)
	{
		for (int yy = -1; yy <= 1; yy ++)
		{
			for (int xx = -1; xx <= 1; xx ++)
			{
				int xPos = x + xx;
				int yPos = y + yy;

				if (world.isInBounds (xPos, yPos))
				{
					territories[yPos, xPos] = faction;
				}
			}
		}
	}

	// if there is a certain faction in the tiles around a point
	public bool checkSurroundingTerritory(int faction, int x, int y)
	{
		for (int yy = -1; yy <= 1; yy ++)
		{
			for (int xx = -1; xx <= 1; xx ++)
			{
				int xPos = x + xx;
				int yPos = y + yy;
				
				if (world.isInBounds (xPos, yPos) &&
				    territories[yPos, xPos] == faction)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int checkTerritory(int x, int y)
	{
		return territories[y, x]; 
	}
}
