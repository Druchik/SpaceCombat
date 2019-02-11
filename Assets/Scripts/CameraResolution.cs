using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSize : MonoBehaviour
{
    public Camera cam;
    public float height, width;
    public bool keepBottomPos = true;
    public Transform thisTransform;

    private float posDiff;
    private Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        thisTransform = this.transform;
        UpdateCameraSize();
    }

    public void UpdateCameraSize()
    {
        cam.orthographicSize = (width / Screen.width * Screen.height) / 2;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        posDiff = height / 2 - cam.orthographicSize;
        targetPos = thisTransform.position - Vector3.up * posDiff;
        if (keepBottomPos) thisTransform.position = targetPos;
    }
}