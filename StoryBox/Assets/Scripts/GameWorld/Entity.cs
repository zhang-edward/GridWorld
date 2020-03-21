using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	[Header("Entity properties")]
	public int maxHealth = 1;
	public int attack;
	public int expandTerritoryRange = 0;
	public List<int> allowedTiles; // Which tiles is this entity allowed to be on?
	public string defaultBehavior;
	public List<string> tags;

	[Header("View")]
	public EntitySprite entitySprite;

	public Vector2Int position { get; private set; }
	public World world { get; private set; }
	public int faction { get; private set; }
	public int health { get; private set; }
	public string uniqueTag { get { return gameObject.GetInstanceID().ToString(); } }
	public Stack<int> currentNodes { get; private set; } // Stores the traversal of the behavior tree to the currently running node

	private Behavior behavior;
	private Memory memory = new Memory();

	public delegate void PositionChanged(Entity entity, Vector2Int oldPos, Vector2Int newPos);
	public event PositionChanged onPositionChanged;

	public delegate void LifecycleEvent(Entity entity);
	public event LifecycleEvent onEntityDied;

	public void Init(int x, int y, int faction, World world) {
		this.faction = faction;
		this.position = new Vector2Int(x, y);
		this.world = world;

		health = maxHealth;
		entitySprite.Init(this);
		transform.position = (Vector2) position;
		currentNodes = new Stack<int>();
		behavior = BehaviorManager.instance.GetBehavior(defaultBehavior);
	}

	public void Act() {
		behavior.Act(this, memory);
	}

	public void Damage(int amt) {
		health -= amt;
	}

	public void Kill() {
		health = 0;
	}

	public void Die() {
		gameObject.SetActive(false);
		// Fire event
		onEntityDied?.Invoke(this);
	}

	public void AssignBehavior(string key) {
		behavior = BehaviorManager.instance.GetBehavior(key);
	}

	public void Move(int x, int y) {
		Vector2Int oldPos = this.position;
		position = new Vector2Int(x, y);
		onPositionChanged?.Invoke(this, oldPos, position);
	}

	void Update() {
		transform.position = Vector3.Lerp(transform.position, (Vector2) position, 0.1f);
	}
}