using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

	public static EntityManager instance;
	public const int MAX_ENTITIES_PER_CELL = 4;

	public GameObject debugEntityPrefab;
	public World world;
	public List<Entity> entities { get; private set; }
	private List<Entity>[, ] entityMap = new List<Entity>[World.WORLD_SIZE, World.WORLD_SIZE];
	public RuntimeObjectPoolerManager entityPools;

	private List<Entity> newEntities = new List<Entity>();

	void Awake() {
		// Singleton, but destroyed on reloaded
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		entities = new List<Entity>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2Int coords = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
			CreateEntity(debugEntityPrefab, coords.x, coords.y, 0);
		}
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
				entityMap[e.position.y, e.position.x].Remove(e);
				e.onPositionChanged -= UpdateEntityMap;
			}
		}
	}

	public Entity CreateEntity(GameObject prefab, int x, int y, int faction) {
		if (CapacityReachedAt(x, y))
			return null;
		// GameObject obj = Instantiate(prefab, this.transform);
		ObjectPooler pool = entityPools.GetPooler(prefab.name);
		if (pool == null)
			pool = entityPools.CreateRuntimeObjectPooler(prefab.name, prefab);
		GameObject obj = pool.GetPooledObject();
		Entity entity = obj.GetComponent<Entity>();
		entity.Init(x, y, faction, world);
		newEntities.Add(entity);
		AddToEntityMap(entity, x, y);
		entity.onPositionChanged += UpdateEntityMap;
		return entity;
	}

	public bool EntityExistsAt(int x, int y) {
		return entityMap[y, x] != null && entityMap[y, x].Count > 0;
	}

	public bool CapacityReachedAt(int x, int y) {
		return entityMap[y, x] != null && entityMap[y, x].Count >= MAX_ENTITIES_PER_CELL;
	}

	public List<Entity> GetEntitiesAt(int x, int y) {
		return entityMap[y, x] != null ? entityMap[y, x] : new List<Entity>();
	}

	public void UpdateEntityMap(Entity entity, Vector2Int oldPos, Vector2Int newPos) {
		entityMap[oldPos.y, oldPos.x].Remove(entity);
		AddToEntityMap(entity, newPos.x, newPos.y);
	}

	public void AddToEntityMap(Entity entity, int x, int y) {
		if (entityMap[y, x] == null) entityMap[y, x] = new List<Entity>(4);
		entityMap[y, x].Add(entity);
	}
}