using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	public int health;
	public int attack;
	public string defaultBehavior;
	// public string deathBehavior;
	public List<int> allowedTiles; // Which tiles is this entity allowed to be on?

	public Vector2Int position { get; private set; }
	public World world { get; private set; }
	public int faction { get; private set; }

	// Stores the traversal of the behavior tree to the currently running node
	public Stack<int> currentNodes { get; private set; }

	private Behavior behavior;
	private Memory memory = new Memory();

	public void Init(int x, int y, int faction, World world) {
		this.faction = faction;
		this.position = new Vector2Int(x, y);
		this.world = world;

		Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };
		GetComponent<SpriteRenderer>().color = colors[faction];

		transform.position = (Vector2) position;
		currentNodes = new Stack<int>();
		behavior = BehaviorManager.instance.GetBehavior(defaultBehavior);
	}

	public void Act() {
		behavior.Act(this, memory);
	}

	public void Die() {
		// if (deathBehavior != null)
		// 	deathBehavior.Act(this, memory);
		gameObject.SetActive(false);
	}

	public void AssignBehavior(string key) {
		behavior = BehaviorManager.instance.GetBehavior(key);
	}

	public void Move(int x, int y) {
		position = new Vector2Int(x, y);
	}

	void Update() {
		transform.position = Vector3.Lerp(transform.position, (Vector2)position, 0.5f);
	}
}