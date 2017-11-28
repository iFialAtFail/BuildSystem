using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SnapPoint : MonoBehaviour
{
    public Collider2D collider;
    private Movable parentMovableObject;
    private SnapPoint closestSnapPointDetected;
    [SerializeField] private GameObject sphereIndicatorPrefab;
    private GameObject sphereIndicator;

    public SnapPoint ClosestSnapPointDetected
    {
        get { return closestSnapPointDetected; }
        private set { closestSnapPointDetected = value; }
    }

    private void Awake()
    {
        parentMovableObject = GetComponentInParent<Movable>();
        if (parentMovableObject == null) Debug.LogWarning("Can't have a Snap Point without a Movable object as the parent game object.");

        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSnappable(bool snappable)
    {
        if (snappable)
        {
            collider.enabled = true;
            Debug.Log("Set collider for " + this.gameObject.name + " to enabled.");
        }
        else
        {
            collider.enabled = false;
            Debug.Log("Set collider for " + this.gameObject.name + " to disabled.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collider.enabled == false) return; //don't need to worry about disabled snapPoints.
        if (collision.gameObject == this.transform.parent.gameObject || collision.gameObject == this.gameObject) return;// Don't collide with parent collider.
        if (collision.transform.parent == null) return;
        var snapPointObjects = collision.transform.parent.gameObject.GetComponentsInChildren<SnapPoint>();

        if (snapPointObjects == null || snapPointObjects.Length == 0) return;

        closestSnapPointDetected = GetClosestSnapPoint(snapPointObjects);


        if (closestSnapPointDetected != null)
        {
            if (sphereIndicator == null) sphereIndicator = Instantiate(sphereIndicatorPrefab, closestSnapPointDetected.transform);
        }
        else if (closestSnapPointDetected == null && sphereIndicator != null)
        {
            Destroy(sphereIndicator.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sphereIndicator != null) Destroy(sphereIndicator.gameObject);
        if (closestSnapPointDetected != null) closestSnapPointDetected = null;
    }

    private SnapPoint GetClosestSnapPoint(SnapPoint[] snapPointObjects)
    {
        if (snapPointObjects.Length == 1) return snapPointObjects[0];

        SnapPoint closestPoint = snapPointObjects[0];
        float distance = float.MaxValue;

        for (int i = 0; i < snapPointObjects.Length; i++)
        {
            var tempDistance = Vector3.Distance(snapPointObjects[i].transform.position, transform.position);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                closestPoint = snapPointObjects[i];
            }
        }
        return closestPoint;
    }

    public void SnapObjects()
    {
        //move snap point doing the detecting to the closest one detected. Make sure whole parent moves with it.
        //var originalChildOffset = transform.localPosition;

        //transform.position = closestSnapPointDetected.transform.position;

        ////Move the parent to the child with the offset.
        //transform.parent.position = transform.position - originalChildOffset;
        //transform.localPosition = originalChildOffset;

        Vector3 theChildsMove = closestSnapPointDetected.transform.position - transform.position;
        //transform.position = closestSnapPointDetected.transform.position;
        transform.parent.position += theChildsMove;
    }
}
