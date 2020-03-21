using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour {
#pragma warning disable CS0649

	public static BehaviorManager instance;
	[SerializeField] private BehaviorTreeDict behaviors;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		
		// Initialize each behavior
		foreach (KeyValuePair<string, Behavior> kvp in behaviors) {
			kvp.Value.Init();
		}
	}

	public Behavior GetBehavior(string key) {
		return behaviors[key];
	}
}