using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour {

	public int x;
	public int y;
	public World world;

	[HideInInspector]
	public bool done;

	void Update()
	{
		transform.localPosition = new Vector2(x, y);
	}

	public abstract void Act();

	public void RemoveEntity()
	{
		world.entities[y, x] = null;
		this.gameObject.SetActive (false);
	}
}
