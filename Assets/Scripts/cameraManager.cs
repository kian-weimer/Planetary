using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 1);
    public Transform target;

    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomSpeed = 10f;

    public GameObject player;

    public Camera camera;
    void Update()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        if (gameObject.GetComponent<Camera>().orthographicSize + scroll * zoomSpeed > 0)
        {
            gameObject.GetComponent<Camera>().orthographicSize += scroll * zoomSpeed;
        }
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

    }
}

