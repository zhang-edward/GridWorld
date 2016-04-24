using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour
{
    private Transform toFollow;
	private bool followingEntity;

    void Start()
    {
		Player.instance.OnChangeEntityControl.AddListener(resetCamera);
    }

	private void resetCamera()
	{
		if (Player.instance.controlledEntity != null)
		{
			toFollow = Player.instance.controlledEntity.GetEntity().transform;
			transform.position = toFollow.position;
			Camera.main.orthographicSize = 8;
			followingEntity = true;
		}
		else
		{
			Camera.main.orthographicSize = World.WORLD_SIZE / 2;
			transform.position = new Vector3(0, -0.5f);
			followingEntity = false;
		}
	}

	void Update()
    {
		if (followingEntity)
		{
			transform.position = Vector3.Lerp(transform.position, toFollow.position, 3.0f * Time.deltaTime); 
		}
	}
}
