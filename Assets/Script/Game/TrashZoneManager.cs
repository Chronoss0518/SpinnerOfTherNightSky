using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashZoneManager : MonoBehaviour
{
    public List<CardScript> trashList {  get; private set; } = new List<CardScript>();

#if UNITY_EDITOR

    public List<CardScript> editorDisplayTrashList;

    void Update()
    {
        editorDisplayTrashList = trashList;
    }

#endif

}
