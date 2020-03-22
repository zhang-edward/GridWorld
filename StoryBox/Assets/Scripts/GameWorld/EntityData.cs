using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity")]
public class EntityData : ScriptableObject {

	[Header("Entity properties")]
	public int maxHealth = 1;
	public int attack;
	public int expandTerritoryRange = 0;
	public List<int> allowedTiles; // Which tiles is this entity allowed to be on?
	public string defaultBehavior;
	public List<string> defaultTags;

	[Header("View")]
	public Sprite sprite;
	public bool immobile;
	public float spriteOffset = 0.3f;
}