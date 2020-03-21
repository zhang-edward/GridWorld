using UnityEngine;
using UnityEngine.Tilemaps;
#pragma warning disable CS0649

public class TerritoryMap : MonoBehaviour {

	public static Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };

	public int[, ] map = new int[World.WORLD_SIZE, World.WORLD_SIZE];
	[SerializeField] private Tilemap baseTilemap;

	void Awake() {
		for (int r = 0; r < World.WORLD_SIZE; r++) {
			for (int c = 0; c < World.WORLD_SIZE; c++) {
				map[r, c] = -1;
			}
		}
	}

	public void Init() {
		
	}
}