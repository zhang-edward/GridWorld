using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour {

	public GameObject commandAnimator;

	public GameObject[] EntityPrefabs;
	public Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();

	void Awake()
	{
		foreach (GameObject o in EntityPrefabs)
		{
			string key = o.name;
			Debug.Log(key);
			dictionary.Add(key, o);
		}
	}
}
