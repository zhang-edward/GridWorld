using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	public static List<ObjectPooler> objectPoolers = new List<ObjectPooler>();

	public bool isGlobal = true;
	public string poolType;
	public GameObject pooledObject;
	public int poolAmount = 10;
	public bool willGrow = true;

	protected List<GameObject> pooledObjects;
	private bool initialized;

	void Awake() {
		Init();
	}

	public virtual void Init() {
		if (initialized)
			return;
		if (isGlobal)
			AddSelfToGlobalList();
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < poolAmount; i++) {
			CreateObject();
		}
		initialized = true;
	}

	protected void AddSelfToGlobalList() {
		for (int i = 0; i < objectPoolers.Count; i++) {
			if (objectPoolers[i].poolType == this.poolType) {
				objectPoolers[i] = this;
				return;
			}
		}
		objectPoolers.Add(this);

	}

	public GameObject GetPooledObject() {
		foreach (GameObject obj in pooledObjects) {
			if (!obj.activeSelf) {
				obj.SetActive(true);
				return obj;
			}
		}

		// Set another `poolAmount` buffer if we need to grow
		if (willGrow) {
			for (int i = 0; i < poolAmount; i++) {
				CreateObject();
			}
			return CreateObject();
		}
		return null;
	}

	private GameObject CreateObject() {
		GameObject obj = Instantiate(pooledObject);
		obj.transform.SetParent(this.transform, false);
		obj.SetActive(false);
		pooledObjects.Add(obj);
		return obj;
	}

	public List<GameObject> GetAllActiveObjects() {
		List<GameObject> ans = new List<GameObject>();
		foreach (GameObject obj in pooledObjects) {
			if (obj.activeSelf)
				ans.Add(obj);
		}
		return ans;
	}

	/// <summary>
	/// Gets the object pooler with the specified name. You should call this in the 'Start' method.
	/// </summary>
	/// <returns>The object pooler.</returns>
	/// <param name="name">Name of the object pooler.</param>
	public static ObjectPooler GetObjectPooler(string name) {
		foreach (ObjectPooler pooler in objectPoolers) {
			if (pooler.poolType == name) {
				return pooler;
			}
		}
		return null;
	}
}