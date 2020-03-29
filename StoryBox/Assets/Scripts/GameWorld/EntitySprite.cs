using System.Collections;
using UnityEngine;

public class EntitySprite : MonoBehaviour {

	// Properties
	private float offsetAmount;
	private bool immobile;

	private SpriteRenderer sr;
	private AnimationSetPlayer player;
	[SerializeField] private SortingLayerControl sortingLayerControl = null;
	private Entity entity;
	private Vector3 randomOffset;
	private float offsetSpeed = 0.05f;

	private Coroutine lerpRoutine;
	private Coroutine damageRoutine;

	void Awake() {
		player = GetComponent<AnimationSetPlayer>();
		sr = GetComponent<SpriteRenderer>();
	}

	public void Init(Entity entity, AnimationSet set, float offsetAmount, bool immobile, bool hasHeight) {
		this.entity = entity;
		this.offsetAmount = offsetAmount;
		this.immobile = immobile;

		// Set this sprite to be behind everything
		if (!hasHeight) {
			sortingLayerControl.usingCustomPivot = true;
			sortingLayerControl.customPivot = 1;
		}

		// Configure animation
		if (set.dict.ContainsKey("Move"))
			set.dict["Move"].SetTimeLength(GameManager.instance.tickInterval);
		player.animationSet = set;
		player.ResetToDefault();

		sr.color = Color.white;
		sr.material.SetColor("Color_C654C1FC", TerritoryMap.instance.colors[entity.faction]);

		randomOffset = new Vector3(Random.Range(-offsetAmount, offsetAmount), Random.Range(-offsetAmount, offsetAmount));
		ChangeOffset();
		entity.onPositionChanged += (unused1, unused2, unused3) => ChangeOffset();
	}

	public void ResetAnimation() {
		player.ResetToDefault();
	}

	public void PlayAnimation(string key) {
		if (player.animationSet.dict.ContainsKey(key)) {
			player.Play(key);
		}
		else
			player.ResetToDefault();
	}

	public void FaceTowards(EntitySprite other) {
		sr.flipX = other.transform.position.x < transform.position.x;
	}

	public void FaceTowards(Vector2Int pos) {
		sr.flipX = pos.x < transform.position.x;
	}

	public void AnimateDamage() {
		if (damageRoutine != null)
			StopCoroutine(damageRoutine);
		damageRoutine = StartCoroutine(DamageRoutine());
	}

	private void ChangeOffset() {
		randomOffset += new Vector3(Random.Range(-offsetSpeed, offsetSpeed), Random.Range(-offsetSpeed, offsetSpeed));
		randomOffset = Vector3.ClampMagnitude(randomOffset, offsetAmount);
		if (immobile)
			transform.localPosition = randomOffset;
		else {
			if (lerpRoutine != null)
				StopCoroutine(lerpRoutine);
			lerpRoutine = StartCoroutine(LerpToOffset());
		}
	}

	private IEnumerator LerpToOffset() {
		while (Vector3.Distance(transform.localPosition, randomOffset) > 0.01f) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, randomOffset, entity.moveLerpSpeed);
			yield return null;
		}
		transform.localPosition = randomOffset;
	}

	private IEnumerator DamageRoutine() {
		float routineLength = GameManager.instance.tickInterval / 2f;
		sr.color = Color.red;
		transform.localPosition += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
		float t = 0;
		while (t < routineLength) {
			t += Time.deltaTime;
			sr.color = Color.Lerp(sr.color, Color.white, t / routineLength);
			transform.localPosition = Vector3.Lerp(transform.localPosition, randomOffset, t / routineLength);
			yield return null;
		}
	}
}