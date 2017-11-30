using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    Camera mainCamera;
    public float zoomSensitivity = 1f;
    public bool reverseZoomScroll = false;
    public float dragSensitivity = 1f;
    private Vector3 offset = Vector3.zero;
    private Vector3 initialPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    private void HandlePan()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            //var rayHit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            //                                        Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
            //                                        Vector2.zero, 0f);
            //var mouseInputVector3 = new Vector3(rayHit.point.x,
            //                                    rayHit.point.y,
            //                                    0);
            //offset = transform.position - mouseInputVector3;
            //initialPos = mouseInputVector3;
            initialPos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {
            var deltaDrag = Input.mousePosition - initialPos;
            //this.transform.position += (deltaDrag + offset);
            this.transform.position += -(deltaDrag * (1/dragSensitivity));
            initialPos = Input.mousePosition;
        }
    }

    private void HandleZoom()
    {
        var zoomAmount = Input.GetAxis("Mouse ScrollWheel");
        if (zoomAmount != 0)
        {
            zoomAmount = reverseZoomScroll ? zoomAmount * 1 : zoomAmount * -1;
            mainCamera.orthographicSize += (zoomSensitivity * zoomAmount);
        }
    }
}
