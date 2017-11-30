using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SnapPoint : MonoBehaviour
{
    public Collider2D snapsCollider;
    

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

        snapsCollider = GetComponent<Collider2D>();
        snapsCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessSnapPointsInTrigger();
    }

   

    private void ProcessSnapPointsInTrigger()
    {
        if (collidedSnapPoints.Count == 0) return;

        if (parentMovableObject.ShouldSnapToObject == false || !parentMovableObject.IsPlacing) return;

        closestSnapPointDetected = GetClosestSnapPoint(collidedSnapPoints.ToArray());


        if (closestSnapPointDetected != null)
        {
            if (sphereIndicator == null)
            {
                sphereIndicator = Instantiate(sphereIndicatorPrefab, closestSnapPointDetected.transform);
            }
            else if (sphereIndicator != null)
            {
                sphereIndicator.transform.SetParent(closestSnapPointDetected.transform);
                sphereIndicator.transform.localPosition = Vector3.zero;
            }
        }
        else if (closestSnapPointDetected == null && sphereIndicator != null)
        {
            Destroy(sphereIndicator.gameObject);
        }
    }
    private List<SnapPoint> collidedSnapPoints = new List<SnapPoint>();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var collidedSnapPoint = collision.GetComponent<SnapPoint>();
        if (collidedSnapPoint == null) return;
        if (!collidedSnapPoints.Contains(collidedSnapPoint))
        {
            collidedSnapPoints.Add(collidedSnapPoint);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sphereIndicator != null) Destroy(sphereIndicator.gameObject);
        if (closestSnapPointDetected != null) closestSnapPointDetected = null;

        var collidedSnapPoint = collision.GetComponent<SnapPoint>();
        if (collidedSnapPoint == null) return;
        if (collidedSnapPoints.Contains(collidedSnapPoint))
        {
            collidedSnapPoints.Remove(collidedSnapPoint);
        }
    }

    private SnapPoint GetClosestSnapPoint(SnapPoint[] snapPointObjects)
    {
        if (snapPointObjects.Length == 1)
            return snapPointObjects[0];

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
        Vector3 theChildsMove = closestSnapPointDetected.transform.position - transform.position;
        parentMovableObject.transform.position += theChildsMove;
        if (sphereIndicator != null) Destroy(sphereIndicator.gameObject);
    }
}
