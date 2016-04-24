using UnityEngine;
using System.Collections;

/// <summary>
/// Objects that appear between turns, usually as a result of an action performed by an entity
/// For example, the fireball shot out of a staff or the explosion of an explosive barrel
/// </summary>
public abstract class IntraTurnObj : MonoBehaviour {

	public Coords pos;
	public bool done;

	public abstract void Activate();
	public abstract void Deactivate();
}
