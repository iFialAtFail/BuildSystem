using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float rotationSensitivity = 5f;

    private Vector3 initialMousePosition;
    private Vector3 offset;
    private MovementManager manager;

    public bool IsPlacing
    {
        get;
        protected set;
    }

    public bool IsRotating
    {
        get;
        protected set;
    }

    public void FollowMouse()
    {
        if (Input.GetMouseButton(0))
        {
            if (manager.snapToGrid)
            {
                var x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                var y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                var pos = new Vector3(x, y, 0); //Where the game object SHOULD be without snapping

                int gridStep = Mathf.RoundToInt(pos.x / gridSize);
                pos.x = ((float)gridStep) * gridSize;

                gridStep = Mathf.RoundToInt(pos.y / gridSize);
                pos.y = ((float)gridStep * gridSize);

                gameObject.transform.position = pos;//After snapping
            }
            else
            {
                var x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                var y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                gameObject.transform.position = new Vector3(x, y, 0) + offset;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsPlacing = false;
        }
    }

    //Add Keyboard Input here and put below FollowMouse in Update method.

    // Update is called once per frame
    void Update()
    {
        if (IsPlacing)
        {
            FollowMouse();
        }
        else if (IsRotating)
        {
            ChangeRotation();
        }
    }

    private void ChangeRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float angle = initialMousePosition.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            transform.Rotate(Vector3.forward, angle * sensitivity);
            initialMousePosition.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }
        else
        {
            IsRotating = false;
        }
    }

    public void SetMovable(Vector3 rayHitLocation)
    {
        offset = transform.position - rayHitLocation;
        IsPlacing = true;
        IsRotating = false;
    }

    public void SetRotating(Vector3 rayHitLocation)
    {
        initialMousePosition = rayHitLocation;
        IsRotating = true;
        IsPlacing = false;
    }

    private void Awake()
    {
        if (manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("MovementManager").GetComponent<MovementManager>();
        }
    }

}
