using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#pragma warning disable CS0649

public class TerritoryMap : MonoBehaviour {

	public static Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };

	[SerializeField] private Tilemap tilemap;
	[SerializeField] private TerritoryTile tile;

	private EntityManager entityManager;

	/// <summary>
	/// First element describes faction, second element describes how many structures are affect
	/// that territory square.
	/// </summary>
	public TerritoryData[, ] map = new TerritoryData[World.WORLD_SIZE, World.WORLD_SIZE];

	public class TerritoryData {
		// Key = number of structures that affect this tile, Value = faction
		public Dictionary<int, int> factions = new Dictionary<int, int>();
		public int control = -1;

		private void UpdateControl() {
			if (factions.Count == 0) {
				control = -1;
			}
			int max;
			factions.TryGetValue(control, out max);
			foreach (KeyValuePair<int, int> kvp in factions) {
				if (kvp.Value > max) {
					max = kvp.Value;
					control = kvp.Key;
				}
			}
		}

		public void IncrementControl(int faction) {
			int val;
			factions.TryGetValue(faction, out val);
			factions[faction] = val + 1;
			UpdateControl();
		}

		public void DecrementControl(int faction) {
			int val;
			factions.TryGetValue(faction, out val);
			factions[faction] = val - 1;
			if (factions[faction] <= 0)
				factions.Remove(faction);
			UpdateControl();
		}
	}

	void Awake() {
		for (int r = 0; r < World.WORLD_SIZE; r++) {
			for (int c = 0; c < World.WORLD_SIZE; c++) {
				map[r, c] = new TerritoryData();
			}
		}
	}

	void Start() {
		entityManager = EntityManager.instance;
		entityManager.onTerritoryExpandingEntityCreated += ExpandTerritory;
	}

	private void ContractTerritory(Entity entity) {
		int range = entity.expandTerritoryRange;
		int xx = entity.position.x;
		int yy = entity.position.y;
		int factionToExpand = entity.faction;
		for (int y = yy - range; y <= yy + range; y++) {
			for (int x = xx - range; x <= xx + range; x++) {
				if (!World.InBounds(x, y)) continue;
				if (entity.position.ManhattanDistance(new Vector2Int(x, y)) <= range) {
					map[y, x].DecrementControl(factionToExpand);
					UpdateTilemap(x, y);
				}
			}
		}
		tilemap.RefreshAllTiles();
		entity.onEntityDied -= ContractTerritory;
	}

	private void ExpandTerritory(Entity entity) {
		int range = entity.expandTerritoryRange;
		int xx = entity.position.x;
		int yy = entity.position.y;
		int factionToExpand = entity.faction;
		for (int y = yy - range; y <= yy + range; y++) {
			for (int x = xx - range; x <= xx + range; x++) {
				if (!World.InBounds(x, y)) continue;
				if (entity.position.ManhattanDistance(new Vector2Int(x, y)) <= range) {
					map[y, x].IncrementControl(factionToExpand);
					UpdateTilemap(x, y);
				}
			}
		}
		tilemap.RefreshAllTiles();
		entity.onEntityDied += ContractTerritory;
	}

	private void UpdateTilemap(int x, int y) {
		Vector3Int pos = new Vector3Int(x, y, 0);
		int control = map[y, x].control;
		if (control == -1) {
			tilemap.SetTile(pos, null);
		} else {
			tilemap.SetTile(pos, tile);
			tilemap.SetTileFlags(pos, TileFlags.LockTransform);
			tilemap.SetColor(pos, colors[control]);
		}
	}
}