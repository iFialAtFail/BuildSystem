using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private Movable currentMovingObject = null;
    public bool snapToGrid = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentMovingObject != null && currentMovingObject.IsPlacing == true) //We are currently Placing, don't need to raycast.
        {
            return;
        } 
        else if (currentMovingObject != null && currentMovingObject.IsPlacing == false)// no longer placing. Forget about the last object and continue raycasting.
        {
            currentMovingObject = null;
        }

        RayCastForMovableObject();

    }
    private void RayCastForMovableObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
                                                    Vector2.zero, 0f);
            if (hit.transform != null)
            {
                var movable = hit.transform.GetComponentInParent<Movable>();
                if (movable != null)
                {
                    currentMovingObject = movable;
                    movable.SetMovable(hit.point);
                }
                else
                {
                    Debug.Log("Click didn't land on a movable object");
                }
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
                                                    Vector2.zero, 0f);
            if (hit.transform != null)
            {
                var movable = hit.transform.GetComponentInParent<Movable>();
                if (movable != null)
                {
                    currentMovingObject = movable;
                    movable.SetRotating(hit.point);
                }
                else
                {
                    Debug.Log("Click didn't land on a movable object");
                }
            }
        }

    }

}
