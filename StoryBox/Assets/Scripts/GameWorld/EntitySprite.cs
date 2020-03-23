using System.Collections;
using UnityEngine;

public class EntitySprite : MonoBehaviour {

	private SpriteRenderer sr;
	private Entity entity;
	private Vector3 randomOffset;
	private float offsetSpeed = 0.05f;

	private AnimationSetPlayer player;

	// Properties
	private float offsetAmount;
	private bool immobile;

	void Awake() {
		player = GetComponent<AnimationSetPlayer>();
		sr = GetComponent<SpriteRenderer>();
	}

	public void Init(Entity entity, AnimationSet set, float offsetAmount, bool immobile) {
		this.entity = entity;
		this.offsetAmount = offsetAmount;
		this.immobile = immobile;
		player.animationSet = set;

		sr.color = TerritoryMap.colors[entity.faction];

		randomOffset = new Vector3(Random.Range(-offsetAmount, offsetAmount), Random.Range(-offsetAmount, offsetAmount));
		ChangeOffset();
		entity.onPositionChanged += (unused1, unused2, unused3) => ChangeOffset();
	}

	public void ResetAnimation() {
		player.ResetToDefault();
	}

	public void PlayAnimation(string key) {
		if (player.animationSet.dict.ContainsKey(key))
			player.Play(key);
		else
			player.ResetToDefault();
	}

	private void ChangeOffset() {
		randomOffset += new Vector3(Random.Range(-offsetSpeed, offsetSpeed), Random.Range(-offsetSpeed, offsetSpeed));
		randomOffset = Vector3.ClampMagnitude(randomOffset, offsetAmount);
	}

	void Update() {
		if (!immobile)
			transform.localPosition = Vector3.Lerp(transform.localPosition, randomOffset, entity.moveLerpSpeed);
		else
			transform.localPosition = randomOffset;
	}
}