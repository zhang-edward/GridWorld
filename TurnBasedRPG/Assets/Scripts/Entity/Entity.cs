using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour {

	public Level level;
	public Coords pos;

	public bool done;

	//protected List<IntraTurnObj> ITObjects = new List<IntraTurnObj>();

	public virtual void Init(Level level)
	{
		this.level = level;
		StartCoroutine("SmoothMovement");
	}

	public abstract void Act();

	private IEnumerator SmoothMovement()
	{
		while (true)
		{
			Vector2 destination = Level.WorldCoord2Position(pos);
			while (Vector2.Distance(transform.position, destination) > 0.1f)
			{
				transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * 20.0f);
				yield return null;
			}
			transform.position = destination;
			yield return null;
		}
	}

	public void Move(Coords dest)
	{
		if (level.getEntity(dest) == null)
		{
			level.entities[pos.y, pos.x] = null;
			level.entities[dest.y, dest.x] = this;
			pos = dest;
		}
	}

	public virtual void Reset()
	{
		done = false;
	}
}
