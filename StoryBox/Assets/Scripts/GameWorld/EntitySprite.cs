using UnityEngine;

public class EntitySprite : MonoBehaviour {

	private SpriteRenderer sr;
	private Entity entity;
	private Vector3 randomOffset;

	// Properties
	private float offsetAmount;
	private bool immobile;

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

	public void Init(Entity entity, Sprite sprite, float offsetAmount, bool immobile) {
		this.entity = entity;
		this.offsetAmount = offsetAmount;
		this.immobile = immobile;

		sr.color = TerritoryMap.colors[entity.faction];
		sr.sprite = sprite;
		ChangeRandomOffset(null, Vector2Int.one, Vector2Int.one);
		entity.onPositionChanged += ChangeRandomOffset;
	}

	private void ChangeRandomOffset(Entity entity, Vector2Int oldPos, Vector2Int newPos) {
		randomOffset = new Vector3(Random.Range(-offsetAmount, offsetAmount), Random.Range(-offsetAmount, offsetAmount));
	}

	void Update() {
		if (!immobile)
			transform.localPosition = Vector3.Lerp(transform.localPosition, randomOffset, entity.moveLerpSpeed);
		else
			transform.localPosition = randomOffset;
	}
}