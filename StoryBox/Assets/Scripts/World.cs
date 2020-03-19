using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {

	public const int WORLD_SIZE = 25;

	// Base tile IDs
	public const int WATER = 0;
	public const int GRASS = 1;
	public const int DIRT = 2;
	public const int SAND = 3;

	// Object tile IDs
	public const int NONE = 0;
	public const int FOREST = 1;
	public const int MOUNTAIN = 2;

	[SerializeField] private Tilemap baseTilemap;
	[SerializeField] private TileBase[] baseTiles;

	[SerializeField] private Tilemap objectTilemap;
	[SerializeField] private TileBase[] objectTiles;

	private WorldGenerator gen = new WorldGenerator();

	private int[, ] baseMap = new int[WORLD_SIZE, WORLD_SIZE];
	private int[, ] objectMap = new int[WORLD_SIZE, WORLD_SIZE];

	public int[, ] BaseMap { get { return baseMap; } }
	public int[, ] ObjectMap { get { return objectMap; } }

	void Awake() {
		GenerateTerrain();

		for (int r = 0; r < WORLD_SIZE; r++) {
			for (int c = 0; c < WORLD_SIZE; c++) {
				baseTilemap.SetTile(new Vector3Int(r, c, 0), baseTiles[baseMap[r, c]]);
				objectTilemap.SetTile(new Vector3Int(r, c, 0), objectTiles[objectMap[r, c]]);
			}
		}
		Camera.main.transform.position = new Vector3(WORLD_SIZE / 2.0f, (WORLD_SIZE - 1) / 2.0f, -10);
		Camera.main.orthographicSize = WORLD_SIZE / 2.0f;
	}

	/// <summary>
	/// Generates the terrain using cell automata.
	/// </summary>
	/// <returns>The array for the terrain</returns>
	/// <param name="size">Size.</param>
	public void GenerateTerrain() {
		gen.randomizeArray(baseMap, 0, 1, 0.45f); // Generate random noise
		gen.reduceNoise(ref baseMap, 1, 10); // Make noise into blobs with cell automata

		// Set borders to be water
		for (int i = 0; i < WORLD_SIZE; i++) {
			baseMap[i, 0] = WATER;
			baseMap[0, i] = WATER;
			baseMap[WORLD_SIZE - 1, i] = WATER;
			baseMap[i, WORLD_SIZE - 1] = WATER;
		}

		gen.checkNeighbors(ref baseMap, GRASS, SAND, WATER, 2);

		gen.overlay(ref baseMap, ref objectMap, FOREST, GRASS, 0.4f, 2);
		gen.overlay(ref baseMap, ref objectMap, MOUNTAIN, GRASS, 0.38f, 2);
		// gen.checkNeighbors(ref map, World.MOUNTAIN, World.DIRT, World.GRASS, 3);
	}
}