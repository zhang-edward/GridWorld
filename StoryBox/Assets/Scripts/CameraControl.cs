using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float zoomSpeed = 0.1f;
    public float cameraMoveSpeed = 0.02f;

    private float targetCameraSize;
    private float maxCameraSize;
    private float minCameraSize = 2;
    private Vector3 targetCameraPos;

    void Start() {
        targetCameraSize = Camera.main.orthographicSize;
        maxCameraSize = Camera.main.orthographicSize;
        targetCameraPos = Camera.main.transform.position;
    }
    // Update is called once per frame
    void Update() {
        float dx = Input.GetAxisRaw("Horizontal");
        float dy = Input.GetAxisRaw("Vertical");
        targetCameraSize = Mathf.Clamp(targetCameraSize + Input.mouseScrollDelta.y * zoomSpeed, minCameraSize, maxCameraSize);
        targetCameraPos = targetCameraPos + new Vector3(dx, dy) * targetCameraSize * cameraMoveSpeed;

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetCameraSize, 0.1f);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetCameraPos, 0.1f);
    }
}