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

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (parentMovableObject.ShouldSnapToObject == false || !parentMovableObject.IsPlacing) return;
        if (collision.transform.parent == null) return;
        var snapPointObjects = collision.transform.parent.gameObject.GetComponentsInChildren<SnapPoint>();

        if (snapPointObjects == null || snapPointObjects.Length == 0) return;

        closestSnapPointDetected = GetClosestSnapPoint(snapPointObjects);


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
        Vector3 theChildsMove = closestSnapPointDetected.transform.position - transform.position;
        transform.parent.position += theChildsMove;
        if (sphereIndicator != null) Destroy(sphereIndicator.gameObject);
    }
}
