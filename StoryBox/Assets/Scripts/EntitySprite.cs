using UnityEngine;

public class EntitySprite : MonoBehaviour {

	private SpriteRenderer sr;
	private Entity entity;
	private Vector3 randomOffset;

	Color[] colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

	public void Init(Entity entity) {
		this.entity = entity;
		sr.color = colors[entity.faction];
		ChangeRandomOffset(null, Vector2Int.one, Vector2Int.one);
		entity.onPositionChanged += ChangeRandomOffset;
	}

	private void ChangeRandomOffset(Entity entity, Vector2Int oldPos, Vector2Int newPos) {
		randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
	}

	void Update() {
		transform.localPosition = Vector3.Lerp(transform.localPosition, randomOffset, 0.1f);
	}
}