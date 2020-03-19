using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public List<Entity> entities;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		foreach (Entity e in entities)
			e.Init();
		StartCoroutine(Loop());
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(0);
	}

	private IEnumerator Loop() {
		for (;;) {
			foreach (Entity e in entities) {
				e.Act();
			}

			foreach (Entity e in entities) {
				if (e.health < 0) {
					e.Die();
					entities.Remove(e);
				}
			}

			yield return new WaitForSecondsRealtime(0.5f);
		}
	}
}