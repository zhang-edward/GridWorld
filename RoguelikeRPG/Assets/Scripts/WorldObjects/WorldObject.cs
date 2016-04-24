using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {

	public int x;
	public int y;

	public World world;

	public string tileName;

	void Awake()
	{
	}
	
	public void InitCoords(int xx, int yy, World world)
	{
		this.world = world;
		x = xx;
		y = yy;
	}


	public bool isInBounds(int x, int y)
	{
		return ((x >= 0 && x < World.WORLD_SIZE) && (y >= 0 && y < World.WORLD_SIZE));
	}
}
