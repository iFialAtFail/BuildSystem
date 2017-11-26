using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackInfo : MonoBehaviour {

    [SerializeField] private GameObject objectToInstantiate;
	
    public GameObject GetObjectToInstantiate()
    {
        return objectToInstantiate;
    }

    private void Awake()
    {
        if (objectToInstantiate.GetComponent<DragHandler>() == null) //All track pieces shoudl be able to be dragged around.
        {
            objectToInstantiate.AddComponent<DragHandler>();
        }
    }
}
