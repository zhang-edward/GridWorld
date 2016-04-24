using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	private LevelManager levelManager;
	private bool shake;
	public float shakeDuration;

	void Start()
	{
		levelManager = LevelManager.instance;
	}

	void Update()
	{
		float gridSize = (float)levelManager.level.gridSize - 1;
		Camera.main.orthographicSize = (gridSize + 1) / 2;

		if (shake)
		{
			shakeDuration -= Time.deltaTime;
			float randX = (Random.value - 0.5f) / 2f;
			float randY = (Random.value - 0.5f) / 2f;
			transform.position = new Vector3(gridSize / 2 + randX, gridSize / 2 + randY, -10);
		}
		else
		{
			transform.position = new Vector3(gridSize / 2, gridSize / 2, -10);
		}

		if (shakeDuration <= 0)
		{
			shake = false;
		}
	}

	public void Shake(float duration)
	{
		shakeDuration = duration;
		shake = true;
	}
}
