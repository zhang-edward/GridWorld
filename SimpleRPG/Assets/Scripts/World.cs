using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
	
	public const int WORLD_SIZE = 30;

	private List<Entity> entitiesList = new List<Entity>();
	private Entity[,] entities = new Entity[WORLD_SIZE, WORLD_SIZE];
	// used for the logic of the board
	public int[,] terrain;
	// only controls the sprites of the terrain
	// TODO: Make this a spriterenderer array
	private GameObject[,] terrainSpriteMap = new GameObject[WORLD_SIZE, WORLD_SIZE];

	public Sprite[] terrainSprites;
	public GameObject terrainPrefab;

	private WorldGenerator wGen;
	private EntityManager eManager;

	public Transform entitiesFolder;
	public Transform terrainFolder;

	void Awake()
	{
		transform.position = new Vector2(-WORLD_SIZE/2, -WORLD_SIZE/2);
		Camera.main.orthographicSize = WORLD_SIZE/2;
		wGen = GetComponent<WorldGenerator>();
		eManager = GetComponent<EntityManager>();
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
			//Debug.Break();
			// DEBUG!! =============================================================
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			// =====================================================================

			/*int counter = 0;*/
			foreach (Entity e in entitiesList)
			{
				if (!e.done)
				{
					e.Act();
					// wait until this entity is done acting
					while (!e.done)
					{
						//Debug.Log(e + "is not done");
						yield return null;
					}
					//counter++;
				}
				/*if (counter % 20 == 0)
					yield return null;*/
			}

            // reset each entity's "done-ness" and remove any dead entities
            for (int i = entitiesList.Count - 1; i >= 0; i --)      // Decrement because we may remove items from the list
            {
                Entity e = entitiesList[i];
                if (e != null)
                    e.done = false;

                if (!e.gameObject.activeSelf)
                {
                    Debug.Log("entity died");
					entities[e.y, e.x] = null;
                    entitiesList.Remove(e);
                }

				// randomize order of entitiesList
				/*int randIndex = Random.Range(0, entitiesList.Count);
				Entity temp = entitiesList[randIndex];
				entitiesList[randIndex] = entitiesList[i];
				entitiesList[i] = temp;*/
            }
			// DEBUG! ==========================================
			watch.Stop();
			//Debug.Log(watch.ElapsedMilliseconds);
			// =================================================

			if (Player.instance.controlledEntity == null)
                yield return new WaitForSeconds(0.5f);
            else
                yield return null;
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

		for (int i = 0; i < 1; i++)
		{
			RandomPlaceEntity(eManager.dictionary["Town"], 1);
			RandomPlaceEntity(eManager.dictionary["Pawn"], 1);
			RandomPlaceEntity(eManager.dictionary["Hero"], 1);
		}
	}

	public Entity RandomPlaceEntity(GameObject prefab, int check)
	{
		int x = Random.Range (0, WORLD_SIZE);
		int y = Random.Range (0, WORLD_SIZE);

		// Debugging purposes only
		int counter = 0;
		// ========================
		//Debug.Log (terrain[y, x]);
		while (!(terrain[y, x] == check && entities[y, x] == null))
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
		//Debug.Log (counter);
		//Debug.Log ("Entity created at: " + x + ", " + y);
		return CreateEntity(prefab, x, y);
	}

	public Entity CreateEntity(GameObject prefab, int x, int y)
	{
		GameObject o = Instantiate(prefab) as GameObject;
		GameObject anim = Instantiate(eManager.commandAnimator) as GameObject;

		o.transform.SetParent(entitiesFolder);
		o.transform.localPosition = new Vector2(x, y);
		//Debug.Log(o.transform.localPosition);

		anim.transform.SetParent(o.transform);
		anim.transform.localPosition = new Vector2(0, 0);
		//Debug.Log(anim.transform.localPosition);

		Entity e = o.GetComponent<Entity>();
		e.anim = anim.GetComponent<CommandAnimator>();
		entities[y, x] = e;
		entitiesList.Add (e);
		e.world = this;
		e.x = x;
		e.y = y;
		e.Init();
		return e;
	}


    // ============================== UTILITY METHODS ===========================================

    /// <summary>
    /// Returns the neighboring entities in the order: up, right, down, left
    /// </summary>
    /// <param name="x">x-coord position</param>
    /// <param name="y">y-coord position</param>
    /// <returns>List of entities</returns>
	public Entity[] getAdjacentEntities(int x, int y)
	{
        Entity[] answer = new Entity[4];
        if (isInBounds(x, y + 1)/* && entities[y + 1, x] != null*/)
            answer[0] = entities[y + 1, x];
        if (isInBounds(x + 1, y) /*&& entities[y, x + 1] != null*/)
			answer[1] = entities[y, x + 1];
		if (isInBounds(x, y - 1) /*&& entities[y - 1, x] != null*/)
			answer[2] = entities[y - 1, x];
        if (isInBounds(x - 1, y)/* && entities[y, x - 1] != null*/)
            answer[3] = entities[y, x - 1];

        // THIS VERSION INCLUDES DIAGONALS
        /*List<Entity> answer = new List<Entity>();
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
		}*/

        return answer;
	}

	public List<int> getAdjacentTerrain(int x, int y)
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

	public bool EntityExistsAt(int x, int y)
	{
		return entities[y, x] != null;
	}

	/// <summary>
	/// Check if the entity can move there first
	/// </summary>
	/// <returns>true if entity successfully moved, false otherwise</returns>
	public bool AttemptMoveEntity(Entity e, int x, int y, List<int> check)
	{
		if (isInBounds(x, y) && 
			check.Contains(terrain[y, x]) &&
			!EntityExistsAt(x, y))
		{
			MoveEntity(e, x, y);
			return true;
		}
		return false;
	}

	public void MoveEntity(Entity e, int x, int y)
	{
		entities[e.y, e.x] = null;
		e.x = x;
		e.y = y;
		entities[y, x] = e;
	}

	public Entity getEntity(int x, int y)
	{ return entities[y, x]; }

	public bool isInBounds(int x, int y)
	{
		if (x < 0 || y < 0)
			return false;
		else if (x >= WORLD_SIZE || y >= WORLD_SIZE)
			return false;
		return true;
	}

    // returns a random entity of some type T
    public T findEntity<T>() where T : Entity
	{
		List<Entity> searchList = new List<Entity>();

		foreach (Entity e in entitiesList)
		{
			// randomize by inserting new object randomly before or after the list
			if (Random.value < 0.5f)
				searchList.Add(e);
			else
				searchList.Insert(0, e);
		}

		/*Debug.Log("=========================");
		foreach (Entity e in searchList)
		{
			Debug.Log(e);
		}
		Debug.Log("=========================");*/

		foreach (Entity e in searchList)
        {
            if (e is T)
                return e as T;
        }
        return null;
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
            SceneManager.LoadScene("Game");
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

	public static Vector3 WorldCoord2Position(int x, int y)
	{
		int xx = x - WORLD_SIZE / 2;
		int yy = y - WORLD_SIZE / 2;
		return new Vector3(xx, yy);
	}
}
