using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

	public static EntityManager instance;
	public List<Entity> entities;

	void Awake() {
		// Singleton, but destroyed on reloaded
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		foreach (Entity e in entities)
			e.Init();
	}

	public void Tick() {
		foreach (Entity e in entities) {
			e.Act();
		}

		for (int i = entities.Count - 1; i >= 0; i--) {
			Entity e = entities[i];
			if (e.health <= 0) {
				e.Die();
				entities.RemoveAt(i);
			}
		}
	}

	public bool EntityExistsAt(int x, int y) {
		foreach (Entity e in entities) {
			if (e.position.x == x && e.position.y == y)
				return true;
		}
		return false;
	}
}