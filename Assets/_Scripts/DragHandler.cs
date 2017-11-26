using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private GameObject spawnItem;
    Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Instantiate prefab version of item user is clicking at the start Position location, assigning the transform parent to a predefined spot on hierarchy
        //Dim Alpha chanel on image of item we are dragging to indicate which one we're dragging
        startPosition = transform.position;
        //newObjectedToPlace = Instantiate(itemInfo.GetObjectToInstantiate());
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.gameObject.transform.position = Input.mousePosition;
        //drag instantiated object around using transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //newObjectedToPlace = null;
        //itemInfo = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var obj = Instantiate(spawnItem);
        var movable = obj.GetComponent<Movable>();
        if (movable != null)
        {
            movable.SetMovable(obj.transform.position);
        }
        else
        {
            Debug.LogWarning("Instantiated object didn't have the movable component.");
        }
    }
}
