using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float HSize=5;
    void Awake()
{
    Camera cam = Camera.main;
    float targetWidth = HSize;
    float screenRatio = (float)Screen.width / Screen.height;
    cam.orthographicSize = targetWidth / (2f * screenRatio);
}

}
