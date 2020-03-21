using System.Collections.Generic;
using UnityEngine;

public class RuntimeObjectPoolerManager : MonoBehaviour {

	public static RuntimeObjectPoolerManager instance;
	public GameObject runtimeObjectPoolerPrefab;

	private Dictionary<string, RuntimeObjectPooler> objectPoolers = new Dictionary<string, RuntimeObjectPooler>();

	void Awake() {
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(this.gameObject);
	}

	public RuntimeObjectPooler CreateRuntimeObjectPooler(string poolName, GameObject prefab, int poolAmount = 10) {
		GameObject o = Instantiate(runtimeObjectPoolerPrefab);
		o.transform.SetParent(transform);

		RuntimeObjectPooler pooler = o.GetComponent<RuntimeObjectPooler>();
		objectPoolers[poolName] = pooler;
		pooler.poolType = poolName;
		pooler.pooledObject = prefab;
		pooler.poolAmount = poolAmount;
		pooler.Init();
		return pooler;
	}

	public RuntimeObjectPooler GetPooler(string poolName) {
		return objectPoolers.ContainsKey(poolName) ? objectPoolers[poolName] : null;
	}
}