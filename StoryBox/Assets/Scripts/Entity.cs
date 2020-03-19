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

	public void Init() {
		position = new Vector2Int(Random.Range(0, World.WORLD_SIZE), Random.Range(0, World.WORLD_SIZE));
		transform.position = (Vector2)position;
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