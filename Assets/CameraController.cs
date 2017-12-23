using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float min;
    public float max;
    public float scrollSpeed;

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        float size;
        size = Camera.main.orthographicSize;
        size -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        size = Mathf.Clamp(size, min, max);
        Camera.main.orthographicSize = size;
    }
}
