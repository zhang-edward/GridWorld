using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity")]
public class EntityData : ScriptableObject {

	[Header("Entity properties")]
	public int maxHealth = 1;
	public int expandTerritoryRange = 0;
	public bool untargetable = false;
	public List<int> allowedTiles; // Which tiles is this entity allowed to be on?
	public string defaultBehavior;
	public string battleBehavior;
	public List<string> defaultTags;

	[Header("View")]
	public AnimationSet animations;
	public bool immobile;
	public bool hasHeight = true;
	public float spriteOffset = 0.3f;
	public InfoPanelData infoPanelData;
}