using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

	public RuntimeObjectPoolerManager entityPools;

	public static EntityManager instance;

	public World world;
	public List<Entity> entities { get; private set; }
	private List<Entity> newEntities = new List<Entity>();

	void Awake() {
		// Singleton, but destroyed on reloaded
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		entities = new List<Entity>();
	}

	public void Tick() {
		// Entity action
		foreach (Entity e in entities) {
			e.Act();
		}

		// Entity creation
		entities.AddRange(newEntities);
		newEntities.Clear();

		// Entity destruction
		for (int i = entities.Count - 1; i >= 0; i--) {
			Entity e = entities[i];
			if (e.health <= 0) {
				e.Die();
				entities.RemoveAt(i);
			}
		}
	}

	public Entity CreateEntity(GameObject prefab, int x, int y, int faction) {
		if (EntityExistsAt(x, y))
			return null;
		// GameObject obj = Instantiate(prefab, this.transform);
		ObjectPooler pool = entityPools.GetPooler(prefab.name);
		if (pool == null)
			pool = entityPools.CreateRuntimeObjectPooler(prefab.name, prefab);
		GameObject obj = pool.GetPooledObject();
		Entity entity = obj.GetComponent<Entity>();
		entity.Init(x, y, faction, world);
		newEntities.Add(entity);
		return entity;
	}

	public bool EntityExistsAt(int x, int y) {
		foreach (Entity e in entities) {
			if (e.position.x == x && e.position.y == y)
				return true;
		}
		return false;
	}
}