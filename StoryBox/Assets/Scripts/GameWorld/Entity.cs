using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	[Header("Debug")]
	public bool debugBehavior;

	[Header("View")]
	public float moveLerpSpeed = 0.05f;
	public EntitySprite entitySprite;

	[Header("Tags")]
	public List<string> tags;

	// Properties
	public int faction { get; private set; }
	public int health { get; private set; }
	public int attack { get; private set; }
	public int expandTerritoryRange { get; private set; }
	public List<int> allowedTiles { get; private set; }

	// State
	public World world { get; private set; }
	public Vector2Int position { get; private set; }
	public string uniqueTag { get { return gameObject.GetInstanceID().ToString(); } }
	public Stack<int> currentNodes { get; private set; } // Stores the traversal of the behavior tree to the currently running node
	public Memory memory { get; private set; }
	public Entity parentEntity { get; private set; }
	public Dictionary<string, List<Entity>> childEntities { get; private set; }

	private Behavior behavior;
	private Coroutine moveRoutine;

	public delegate void PositionChanged(Entity entity, Vector2Int oldPos, Vector2Int newPos);
	public event PositionChanged onPositionChanged;

	public delegate void LifecycleEvent(Entity entity);
	public event LifecycleEvent onEntityInitialized;
	public event LifecycleEvent onEntityDied;

	public void Init(int x, int y, int faction, World world, EntityData data) {
		this.faction = faction;
		this.world = world;

		position = new Vector2Int(x, y);
		childEntities = new Dictionary<string, List<Entity>>();

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
		entitySprite.Init(this, data.animations, data.spriteOffset, data.immobile);
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
			//foreach (int n in btTraversal)
			//	print(n);
			print("Next tick:\n" + behavior.PrintTreeTraversal(btTraversal, this));
		}
	}

	public void Damage(int amt) {
		health -= amt;
		entitySprite.AnimateDamage();
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
		FaceTowards(position);

		if (moveRoutine != null)
			StopCoroutine(moveRoutine);
		moveRoutine = StartCoroutine(MoveRoutine(transform.position, new Vector3(x, y)));
		onPositionChanged?.Invoke(this, oldPos, position);
	}

	public override string ToString() {
		return gameObject.name;
	}

	private IEnumerator MoveRoutine(Vector3 oldPos, Vector3 newPos) {
		PlayAnimation("Move");
		float t = 0;
		float moveTime = GameManager.instance.tickInterval;
		while (t < moveTime) {
			t += Time.deltaTime;
			transform.position = Vector2.Lerp(oldPos, newPos, t / moveTime);
			yield return null;
		}
		transform.position = newPos;
		yield return null;
	}

	public void PlayAnimation(string key) {
		entitySprite.PlayAnimation(key);
	}

	public void ResetAnimation() {
		entitySprite.ResetAnimation();
	}

	public void FaceTowards(Entity other) {
		entitySprite.FaceTowards(other.entitySprite);
	}

	public void FaceTowards(Vector2Int pos) {
		entitySprite.FaceTowards(pos);
	}

	public void SetParent(Entity parentEntity) {
		this.parentEntity = parentEntity;
	}

	public void AddChild(string key, Entity child) {
		if (!childEntities.ContainsKey(key)) {
			childEntities[key] = new List<Entity>();
			memory[key] = childEntities[key];
		}
		childEntities[key].Add(child);
		child.SetParent(this);
		child.onEntityDied += RemoveChild;
	}

	public void RemoveChild(Entity child) {
		foreach (List<Entity> list in childEntities.Values)
			if (list.Contains(child))
				list.Remove(child);
		child.onEntityDied -= RemoveChild;
	}
}