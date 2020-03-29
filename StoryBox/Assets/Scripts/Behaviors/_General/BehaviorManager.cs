using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour {
#pragma warning disable CS0649

	public static BehaviorManager instance;
	[SerializeField] private BehaviorTreeDict entityBehaviors;
	[SerializeField] private BehaviorTreeDict structureBehaviors;
	[SerializeField] private BehaviorTreeDict battleBehaviors;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		
		// Initialize each behavior
		foreach (KeyValuePair<string, Behavior> kvp in entityBehaviors)
			kvp.Value.Init();
		foreach (KeyValuePair<string, Behavior> kvp in structureBehaviors)
			kvp.Value.Init();
		foreach (KeyValuePair<string, Behavior> kvp in battleBehaviors)
			kvp.Value.Init();
	}

	public Behavior GetBehavior(string key) {
		if (entityBehaviors.ContainsKey(key))
			return entityBehaviors[key];
		else if (structureBehaviors.ContainsKey(key))
			return structureBehaviors[key];
		else if (battleBehaviors.ContainsKey(key))
			return battleBehaviors[key];
		else {
			Debug.LogError($"Couldn't find behavior {key}");
			return null;
		}
	}
}