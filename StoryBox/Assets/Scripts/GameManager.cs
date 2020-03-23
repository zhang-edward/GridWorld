using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public float tickInterval = 0.5f;

	private bool paused = true;
	private bool step = false;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		StartCoroutine(Loop());
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(0);
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			paused = !paused;
		}
		if (Input.GetKeyDown(KeyCode.Period)) {
			step = true;
			paused = false;
		}
	}

	private IEnumerator Loop() {
		for (;;) {
			while (paused) {
				step = false;
				yield return null;
			}
			EntityManager.instance.Tick();
			if (step)
				paused = true;
			yield return new WaitForSecondsRealtime(tickInterval);
		}
	}

	// private async void Loop() {
	// 	for (;;) {
	// 		EntityManager.instance.Tick();
	// 		await Task.Delay(500);
	// 	}
	// }
}