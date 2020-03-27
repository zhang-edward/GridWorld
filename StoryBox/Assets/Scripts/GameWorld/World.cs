using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#pragma warning disable CS0649

public class World : MonoBehaviour {

	public const int WORLD_SIZE = 50;

	// Base tile IDs
	public const int WATER = 0;
	public const int SAND = 1;
	public const int GRASS = 2;
	public const int DIRT = 3;

	// Object tile IDs
	public const int NONE = 0;
	public const int FOREST1 = 1;
	public const int FOREST2 = 2;
	public const int FOREST3 = 3;
	public const int FOREST4 = 4;
	public const int MOUNTAIN = 5;


	[SerializeField] private Tilemap resourcesTilemap;
	[SerializeField] private TileBase[] resourcesTiles;

	[SerializeField] private GameObject tilemapPrefab;
	[SerializeField] private Tilemap[] baseTilemaps;
	[SerializeField] private TileBase[] baseTiles;
	public float[] thresholds = new float[] { 0.12f, 0.2f, 0.4f };

	public EntityData entityData;

	private WorldGenerator gen = new WorldGenerator();

	private int[, ] baseMap = new int[WORLD_SIZE, WORLD_SIZE];
	private int[, ] resourcesMap = new int[WORLD_SIZE, WORLD_SIZE];

	public int[, ] BaseMap { get { return baseMap; } }
	public int[, ] ResourcesMap { get { return resourcesMap; } }

	void Awake() {
		Camera.main.transform.position = new Vector3(WORLD_SIZE / 2.0f, (WORLD_SIZE - 1) / 2.0f, -10);
		Camera.main.orthographicSize = WORLD_SIZE / 2.0f;

		GenerateTerrain();

		baseTilemaps = new Tilemap[baseTiles.Length];
		for (int i = 0; i < baseTiles.Length; i++) {
			GameObject obj = Instantiate(tilemapPrefab, transform);
			Tilemap tilemap = obj.GetComponent<Tilemap>();
			baseTilemaps[i] = tilemap;
		}

		for (int y = 0; y < WORLD_SIZE; y++) {
			for (int x = 0; x < WORLD_SIZE; x++) {
				for (int i = 0; i < baseTilemaps.Length; i++) {
					if (i <= baseMap[y, x])
						baseTilemaps[i].SetTile(new Vector3Int(x, y, 0), baseTiles[i]);
				}
				resourcesTilemap.SetTile(new Vector3Int(x, y, 0), resourcesTiles[resourcesMap[y, x]]);
			}
		}
	}

	void Start() {
		for (int i = 0; i < 1; i++) {
			int limit = 0;
			while (limit < 1000) {
				int x = Random.Range(0, World.WORLD_SIZE);
				int y = Random.Range(0, World.WORLD_SIZE);
				if (baseMap[y, x] == GRASS) {
					EntityManager.instance.CreateEntity(entityData, x, y, i);
					break;
				}
				limit++;
			}
		}
	}

	public void ModifyStructureMap(int x, int y, int newTile) {
		resourcesMap[y, x] = newTile;
		resourcesTilemap.SetTile(new Vector3Int(x, y, 0), resourcesTiles[newTile]);
	}

	/// <summary>
	/// Generates the terrain using cell automata.
	/// </summary>
	/// <returns>The array for the terrain</returns>
	/// <param name="size">Size.</param>
	public void GenerateTerrain() {
		gen.GeneratePerlinNoiseMap(ref baseMap, WORLD_SIZE, thresholds);

		// Set borders to be water
		for (int i = 0; i < WORLD_SIZE; i++) {
			baseMap[i, 0] = WATER;
			baseMap[0, i] = WATER;
			baseMap[WORLD_SIZE - 1, i] = WATER;
			baseMap[i, WORLD_SIZE - 1] = WATER;
		}

		gen.CheckNeighbors(ref baseMap, GRASS, SAND, WATER, 2);

		gen.Overlay(ref baseMap, ref resourcesMap, FOREST1, GRASS, 0.6f, 2);
		gen.CheckNeighbors(ref resourcesMap, FOREST1, FOREST2, FOREST1, 7);
		gen.CheckNeighbors(ref resourcesMap, FOREST2, FOREST3, FOREST2, 6);
		gen.CheckNeighbors(ref resourcesMap, FOREST3, FOREST4, FOREST3, 6);

		gen.Overlay(ref baseMap, ref resourcesMap, MOUNTAIN, DIRT, 0.5f, 2);
		//gen.checkNeighbors(ref map, World.MOUNTAIN, World.DIRT, World.GRASS, 3);
	}

	public static List<Vector2Int> GetAdjacentCoords(int x, int y) {
		List<Vector2Int> ans = new List<Vector2Int>();
		int xx, yy;

		xx = x + 1;
		yy = y;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x;
		yy = y + 1;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x - 1;
		yy = y;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		xx = x;
		yy = y - 1;
		if (InBounds(xx, yy)) ans.Add(new Vector2Int(xx, yy));
		return ans;
	}

	public static bool InBounds(int x, int y) {
		return x >= 0 && x < World.WORLD_SIZE && y >= 0 && y < World.WORLD_SIZE;
	}
}