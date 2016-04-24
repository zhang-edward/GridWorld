using UnityEngine;
using System.Collections;

public abstract class Entity : WorldObject {

	public int health;
	public int attack;
	public int defense;

	// initialize stats
	public void Init(int health, int attack, int defense)
	{
		this.health = health;
		this.attack = attack;
		this.defense = defense;
	}
	
	void Update()
	{
		// TEMPORARY
		if (health <= 0)
		{
			this.gameObject.SetActive (false);
		}
	}
	
	void OnDisable()
	{
		Debug.Log("Entity disabled");

		// remove this entity from the world list so that it no longer registers as an entity
		world.entityLayer.Remove (this);
	}

	// update the transform position to match the x and y coords in the World data
	protected void UpdatePos()
	{transform.position = new Vector3(x, y);}

	// All AI entities will call this method when it is their turn
	// Any AI logic goes in this method
	public abstract void Act();
}
