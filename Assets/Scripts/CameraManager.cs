using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float startZoom = 30.0f;
    public float zoomSpeed = 1.5f;
    public float zoomMin = 10.0f;
    public float zoomMax = 80.0f;

    private Vector3 MouseDownPosition;

    void Start()
    {
        Camera.main.orthographicSize = startZoom;
    }

    void Update()
    {
        // If mouse wheel scrolled vertically, apply zoom...
        // TODO: Add pinch to zoom support (touch input)
        if (Input.mouseScrollDelta.y != 0)
        {
            // Save location of mouse prior to zoom
            var preZoomPosition = getWorldPoint(Input.mousePosition);

            // Apply zoom (might want to multiply Input.mouseScrollDelta.y by some speed factor if you want faster/slower zooming
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (Input.mouseScrollDelta.y * zoomSpeed), zoomMin, zoomMax);

            // How much did mouse move when we zoomed?
            var delta = getWorldPoint(Input.mousePosition) - preZoomPosition;

            // Move the camera by the amount mouse moved, so that mouse is back in same position now.
            Camera.main.transform.Translate(-delta.x, -delta.y, 0);
        }

        // When mouse is first pressed, just save location of mouse/finger.
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownPosition = getWorldPoint(Input.mousePosition);
        }

        // While mouse button/finger is down...
        if (Input.GetMouseButton(0))
        {
            // Total distance finger/mouse has moved while button is down
            var delta = getWorldPoint(Input.mousePosition) - MouseDownPosition;

            // Adjust camera by distance moved, so mouse/finger stays at exact location (in world, since we are using getWorldPoint for everything).
            Camera.main.transform.Translate(-delta.x, -delta.y, 0);
        }
    }

    private Vector3 getWorldPoint(Vector2 screenPoint)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
        return hit.point;
    }
}
