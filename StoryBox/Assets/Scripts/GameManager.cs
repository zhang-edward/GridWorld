using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		Loop();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(0);
		}
	}

	// private IEnumerator Loop() {
	// 	for (;;) {
	// 		EntityManager.instance.Tick();
	// 		yield return new WaitForSecondsRealtime(0.5f);
	// 	}
	// }

	private async void Loop() {
		for (;;) {
			EntityManager.instance.Tick();
			await Task.Delay(500);
		}
	}
}