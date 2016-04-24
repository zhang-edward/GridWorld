using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Entity : MonoBehaviour {

	public int x;
	public int y;
	public World world;
	public List<int> canMove = new List<int>();

	public CommandAnimator anim;

	// to prevent the same entity being selected twice after moving to the right or down
	[HideInInspector] public bool done;

	public abstract void Act();

	public void DisableSelf()
	{
		//world.entities[y, x] = null;
		this.gameObject.SetActive (false);
	}

	public Entity GetEntity()
	{
		return this;
	}

	public void Init()
	{
		StartCoroutine("SmoothMovement");
	}

	public IEnumerator SmoothMovement()
	{
		while(true)
		{
			Vector2 destination = World.WorldCoord2Position(x, y);
			while (Vector2.Distance(transform.position, destination) > 0.1f)
			{
				transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * 20.0f);
				yield return null;
			}
			transform.position = destination;
			yield return null;
		}

	}
}
