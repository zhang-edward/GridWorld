using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

	public static EntityManager instance;
	public const int MAX_ENTITIES_PER_CELL = 100;

	public EntityData debugEntity1;
	public EntityData debugEntity2;
	[Space]
	public World world;
	public List<Entity> entities { get; private set; }
	public ObjectPooler entityPool;

	public delegate void EntityEvent(Entity entity);
	public event EntityEvent onTerritoryExpandingEntityCreated;

	private List<Entity>[, ] entityMap = new List<Entity>[World.WORLD_SIZE, World.WORLD_SIZE];
	private List<Entity> newEntities = new List<Entity>();

	void Awake() {
		// Singleton, but destroyed on reloaded
		if (instance != this)
			instance = this;

		entities = new List<Entity>();
	}

	void Start() {
		GameManager.instance.onTick += Tick;
	}

	void OnDisable() {
		GameManager.instance.onTick -= Tick;
	}

	void Update() {
		Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2Int coords = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			CreateEntity(debugEntity1, coords.x, coords.y, 0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			CreateEntity(debugEntity2, coords.x, coords.y, 0);
		}
		if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) {
			List<Entity> l = entityMap[coords.y, coords.x];
			for (int i = l.Count - 1; i >= 0; i--) {
				Entity e = l[i];
				e.Damage(e.maxHealth / 2);
			}
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

	public Entity CreateEntity(EntityData data, int x, int y, int faction) {
		// Check preconditions for spawning entity
		if (CapacityReachedAt(x, y))
			return null;

		// Configure GameObject
		GameObject obj = entityPool.GetPooledObject();
		Entity entity = obj.GetComponent<Entity>();

		// Track entity
		newEntities.Add(entity);
		AddToEntityMap(entity, x, y);
		entity.onPositionChanged += UpdateEntityMap;
		entity.onEntityInitialized += EntityInitialized;

		// Initialize entity
		entity.Init(x, y, faction, world, data);

		return entity;
	}

	private void EntityInitialized(Entity entity) {
		if (entity.expandTerritoryRange > 0)
			onTerritoryExpandingEntityCreated?.Invoke(entity);
	}

	public void RemoveEntity(Entity e) {
		e.Die();
		entities.Remove(e);
		entityMap[e.position.y, e.position.x].Remove(e);
		e.onPositionChanged -= UpdateEntityMap;
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