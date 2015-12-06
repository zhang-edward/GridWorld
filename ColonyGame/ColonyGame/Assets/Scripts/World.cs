using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public const int WORLD_SIZE = 55;
	
	public Entity[,] entities = new Entity[WORLD_SIZE, WORLD_SIZE];
	// used for the logic of the board
	public int[,] terrain;
	// only controls the sprites of the terrain
	// TODO: Make this a spriterenderer array
	public GameObject[,] terrainSpriteMap = new GameObject[WORLD_SIZE, WORLD_SIZE];

	public Sprite[] terrainSprites;
	public GameObject terrainPrefab;
	public GameObject[] entityPrefabs;

	private WorldGenerator wGen;

	public Transform entitiesFolder;
	public Transform terrainFolder;

	void Awake()
	{
		transform.position = new Vector2(-WORLD_SIZE/2, -WORLD_SIZE/2);
		Camera.main.orthographicSize = WORLD_SIZE/2;
		wGen = GetComponent<WorldGenerator>();
	}

	void Start()
	{
		GenerateWorld();
		StartCoroutine("GameLoop");
	}

	// TODO: add a list of entities to remove the necessity of "done" variable
	IEnumerator GameLoop()
	{
		while (true)
		{
			foreach (Entity e in entities)
			{
				if (e != null && !e.done)
				{
					e.Act ();
					// to prevent the same entity being selected twice after moving to the right or down
					e.done = true;
				}
			}

			foreach (Entity e in entities)
				if (e != null)
					e.done = false;
			yield return new WaitForSeconds(0.2f);
		}
	}

	void GenerateWorld()
	{
		terrain = wGen.GenerateTerrain(WORLD_SIZE);
		for (int y = 0; y < WORLD_SIZE; y ++)
		{
			for (int x = 0; x < WORLD_SIZE; x ++)
			{
				GameObject o = Instantiate(terrainPrefab, Vector3.one, Quaternion.identity) as GameObject;
				o.transform.SetParent (terrainFolder);
				o.transform.localPosition = new Vector2(x, y);

				terrainSpriteMap[y, x] = o;
				int tileID = terrain[y, x];
				o.GetComponent<SpriteRenderer>().sprite = terrainSprites[tileID];
			}
		}
		for (int i = 0; i < 2; i ++)
		{
			ColonyCenter colony = RandomPlaceEntity(0, 1) as ColonyCenter;
			colony.territory = GetComponent<TerritoryMngr>();
			colony.Init (i);
		}
	}

	public Entity RandomPlaceEntity(int id, int check)
	{
		int x = Random.Range (0, WORLD_SIZE);
		int y = Random.Range (0, WORLD_SIZE);

		// Debugging purposes only
		int counter = 0;
		// ========================
		while (terrain[y, x] != check)
		{
			x = Random.Range (0, WORLD_SIZE);
			y = Random.Range (0, WORLD_SIZE);

			// Debugging purposes only (prevent infinite loop)
			if (counter > 1000)
			{
				Debug.LogWarning ("Randomly placing an entity on tile with tileID: " + check + " is taking 1000+ tries, breaking");
				break;
			}
			counter ++;
			// =========================
		}
		//Debug.Log ("Entity created at: " + x + ", " + y);
		return CreateEntity(id, x, y);
	}

	public Entity CreateEntity(int id, int x, int y)
	{
		GameObject o = Instantiate(entityPrefabs[id], Vector3.one, Quaternion.identity) as GameObject;
		o.transform.SetParent(entitiesFolder);
		o.transform.localPosition = new Vector2(x, y);

		Entity e = o.GetComponent<Entity>();
		entities[y, x] = e;
		e.world = this;
		e.x = x;
		e.y = y;
		return e;
	}

	public List<Entity> getNeighboringEntities(int x, int y)
	{
		List<Entity> answer = new List<Entity>();
		for (int yy = -1; yy <= 1; yy ++)
		{
			for (int xx = -1; xx <= 1; xx ++)
			{
				// coords in array to be checked
				int xCheck = x + xx;
				int yCheck = y + yy;
				// do not check at middle coords
				if (!(xx == 0 && yy == 0))
				{
					if (isInBounds(xCheck, yCheck) &&
					    entities[yCheck, xCheck] != null)
						answer.Add (entities[yCheck, xCheck]);
				}

			}
		}
		return answer;
	}

	public List<int> getNeighboringTerrain(int x, int y)
	{
		List<int> answer = new List<int>();
		if (isInBounds(x + 1, y))
			answer.Add (terrain[y, x + 1]);
		if (isInBounds(x - 1, y))
			answer.Add (terrain[y, x - 1]);
		if (isInBounds(x, y + 1))
			answer.Add (terrain[y + 1, x]);
		if (isInBounds(x, y - 1))
			answer.Add (terrain[y - 1, x]);
		return answer;
	}

	/// <summary>
	/// Gets the adjacent coords that are in bounds.
	/// </summary>
	/// <returns>The adjacent coords.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public List<Vector2> getAdjacentCoords(int x, int y)
	{
		List<Vector2> answer = new List<Vector2>();
		if (isInBounds(x + 1, y))
			answer.Add (new Vector2(x + 1, y));
		if (isInBounds(x - 1, y))
			answer.Add (new Vector2(x - 1, y));
		if (isInBounds(x, y + 1))
			answer.Add (new Vector2(x, y + 1));
		if (isInBounds(x, y - 1))
			answer.Add (new Vector2(x, y - 1));
		return answer;
	}

	public bool isInBounds(int x, int y)
	{
		if (x < 0 || y < 0)
			return false;
		else if (x > WORLD_SIZE || y > WORLD_SIZE)
			return false;
		return true;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel (Application.loadedLevel);
		}

		// DEBUG
		/*for (int y = 0; y < WORLD_SIZE; y ++)
		{
			for (int x = 0; x < WORLD_SIZE; x ++)
			{
				if (entities[y, x] != null)
					terrainSpriteMap[y, x].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
				else
					terrainSpriteMap[y, x].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
			}
		}*/
	}
}
