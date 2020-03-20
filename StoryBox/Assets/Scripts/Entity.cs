using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour {

	public int health;
	public int faction;
	public World world;
	public Behavior behavior; // Root of the behavior tree
	public Behavior deathBehavior;

	public Vector2Int position { get; private set; }

	private Coroutine smoothMoveRoutine;
	private Memory memory = new Memory();

	public void Init(int x, int y, int faction, World world) {
		this.faction = faction;
		this.position = new Vector2Int(x, y);
		this.world = world;

		Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };
		GetComponent<SpriteRenderer>().color = colors[faction];

		transform.position = (Vector2) position;
		behavior.Init(this, memory);
	}

	public void Act() {
		behavior.Act();
	}

	public void Die() {
		if (deathBehavior != null)
			deathBehavior.Act();
		gameObject.SetActive(false);
	}

	public void Move(int x, int y) {
		position = new Vector2Int(x, y);
		if (smoothMoveRoutine != null)
			StopCoroutine(smoothMoveRoutine);
		smoothMoveRoutine = StartCoroutine(SmoothMove(x, y));
	}

	private IEnumerator SmoothMove(int x, int y) {
		float t = 0f;
		Vector3 dest = new Vector3(x, y);
		while (t < 0.5f) {
			t += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, dest, t);
			yield return null;
		}
		transform.position = dest;
	}
}