using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	[Header("Debug")]
	public bool debugBehavior;

	[Header("View")]
	public float moveLerpSpeed = 0.05f;
	public EntitySprite entitySprite;

	public int faction { get; private set; }
	public int health { get; private set; }
	public int attack { get; private set; }
	public int expandTerritoryRange { get; private set; }
	public List<string> tags { get; private set; }
	public List<int> allowedTiles { get; private set; }
	public World world { get; private set; }
	public Vector2Int position { get; private set; }
	public string uniqueTag { get { return gameObject.GetInstanceID().ToString(); } }
	public Stack<int> currentNodes { get; private set; } // Stores the traversal of the behavior tree to the currently running node
	public Memory memory { get; private set; }

	private Behavior behavior;

	public delegate void PositionChanged(Entity entity, Vector2Int oldPos, Vector2Int newPos);
	public event PositionChanged onPositionChanged;

	public delegate void LifecycleEvent(Entity entity);
	public event LifecycleEvent onEntityInitialized;
	public event LifecycleEvent onEntityDied;

	public void Init(int x, int y, int faction, World world, EntityData data) {
		this.faction = faction;
		this.position = new Vector2Int(x, y);
		this.world = world;

		memory = new Memory();
		memory["self"] = this;

		TransformTo(data);
		// Other properties
		transform.position = (Vector2) position;
	}

	public void TransformTo(EntityData data) {
		allowedTiles = data.allowedTiles;
		attack = data.attack;
		health = data.maxHealth;
		expandTerritoryRange = data.expandTerritoryRange;
		entitySprite.Init(this, data.sprite, data.spriteOffset, data.immobile);
		behavior = BehaviorManager.instance.GetBehavior(data.defaultBehavior);
		tags = new List<string>();
		tags.AddRange(data.defaultTags);

		// Reset behavior tree
		currentNodes = new Stack<int>();

		// Configure GameObject
		gameObject.name = data.name + gameObject.GetInstanceID();

		// Fire events
		onEntityInitialized?.Invoke(this);
	}

	public void Act() {
		behavior.Act(this, memory);
		if (debugBehavior) {
			// Perform shallow copy of the stack
			List<int> lst = new List<int>(currentNodes.ToArray());
			lst.Reverse();
			Stack<int> btTraversal = new Stack<int>(lst);
			print(behavior.PrintTreeTraversal(btTraversal, this));
		}
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

	public override string ToString() {
		return gameObject.name;
	}

	void Update() {
		transform.position = Vector3.Lerp(transform.position, (Vector2) position, moveLerpSpeed);
	}
}